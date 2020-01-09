using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopManager.net.Utils;
using WorkshopManagerNET.Model;

namespace WorkshopManagerNET.Model
{
  partial class Order
  {
    public override string ToString()
    {
      return $"{Title} {VehicleDescription} {DateRegister.ToShortDateString()}";
    }  
  }
}

namespace WorkshopManager.net.DataGenerator
{
  class OrderData : IDataGenerator<Order>
  {
    private JsonModelsReader<Order> _reader = null;
    private Order[] _models = null;
    private const string _jsonFileName = "orders.sample-data.json";

    public JsonModelsReader<Order> JsonReader
    {
      get
      {
        if (_reader == null)
        {
          _reader = new JsonModelsReader<Order>(_jsonFileName);
        }
        return _reader;
      }
      set => _reader = value;
    }
    public Order[] Models
    {
      get
      {
        if (_models == null)
        {
          LoadModels();
        }
        return _models;
      }
      set => _models = value;
    }
    public async Task<bool> InsertModelsAsync()
    {
      try
      {
        using (var dbAccess = new WorkshopManagerContext())
        {
          #region Clients Get || Insert & Get
          Client[] clients = null;         
          clients = dbAccess.Clients.ToArray();
          if (clients.Length == 0)
          {
            var insertSucceeded = await InsertClients();
            if (insertSucceeded)
            {
              clients = dbAccess.Clients.ToArray();
            }
            else
            {
              Console.WriteLine("Could not insert Clients... (necessary if Order's are about to be inserted)");
              return false;
            }
          }
          #endregion

          for(int i = 0; i<clients.Length; i++)
          {
            var order = Models[i];
            order.ClientId = i;
            order.Client = clients[i];
            order.Status = OrderStatusEnum.Registered;
            order.DateRegister = DateTime.Now;
          }
          dbAccess.BulkInsert(Models.Where(m=>m.Client != null).ToArray());
          dbAccess.SaveChanges();
          return true;
        }
      }
      catch(Exception e)
      {
        Console.WriteLine(e);
        return false;
      }
    }

    public async Task<bool> InsertClients()
    {
      var clientsGenerator = new ClientData();
      return await clientsGenerator.InsertModels();
    }


    public void LoadModels()
    {
      Models = JsonReader.GetModels().ToArray();
    }

    public bool InsertModels()
    {
      throw new NotImplementedException();
    }
  }
}
