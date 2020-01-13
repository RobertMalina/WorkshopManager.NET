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
  class OrderCase
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public string VehicleDescription { get; set; }
    public ComplexityClassEnum Complexity { get; set; }
  }
  class OrderData : IDataGenerator<Order>
  {
    private JsonModelsReader<Order> _reader = null;
    private JsonModelsReader<OrderCase> _casesReader = new JsonModelsReader<OrderCase>(_orderCasesjsonFileName);

    private Order[] _models = null;
    private const string _jsonFileName = "orders.sample-data.json";
    private const string _orderCasesjsonFileName = "order-cases.json";

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
      var sampleCases = _casesReader.GetModels().ToArray();
      var orders = new List<Order>();
      foreach (OrderCase orderCase in sampleCases)
        orders.Add(new Order()
        {
          Title = orderCase.Title,
          VehicleDescription = orderCase.VehicleDescription,
          DateRegister = DateTime.Now,
          ComplexityClass = orderCase.Complexity,
          Description = orderCase.Description
        });

      Models = orders.ToArray();
    }

    public bool InsertModels()
    {
      throw new NotImplementedException();
    }
  }
}
