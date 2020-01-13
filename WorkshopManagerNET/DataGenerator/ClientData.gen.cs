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
  public partial class Client
  {
    public override string ToString()
    {
      return $"{FirstName} {LastName} {PhoneNumber}";
    }
  }
}

namespace WorkshopManager.net.DataGenerator
{
  class ClientData : IDataGenerator<Client>
  {
    private const string _jsonFileName = "clients.sample-data.json";

    private JsonModelsReader<Client> _reader = null;
    private Client[] _models = null;
    public JsonModelsReader<Client> JsonReader
    {
      get
      {
        if (_reader == null)
        {
          _reader = new JsonModelsReader<Client>(_jsonFileName);
        }
        return _reader;
      }
      set => _reader = value;
    }

    public Client[] Models
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

    public bool InsertModels()
    {
      try
      {
        using(var dbAccess = new WorkshopManagerContext())
        {
          dbAccess.BulkInsertAsync(Models);
          dbAccess.SaveChanges();
        }
        return true;
      }
      catch(Exception e)
      {
        Console.WriteLine(e);
        return false;
      }
    }

    public void MatchRandomlyWith(Order[] orders)
    {
      LoadDbModels();
      var clientIdsOfActiveOrders = new List<long>(); //-> because there shouldn't exists more than one active order (Registered || InProgress) of particullar client
      var rand = new Random();
      foreach (Order order in orders)
      {
        var client = GetRandom();
        switch (order.Status)
        {
          case OrderStatusEnum.InProgress:
          case OrderStatusEnum.Registered:
            {
              if (clientIdsOfActiveOrders.Contains(client.Id))
              {
                order.Status = OrderStatusEnum.Finished; //-> because there shouldn't exists more than one active order (Registered || InProgress) of particullar client
              }
              order.ClientId = client.Id;
              order.Client = client;
              clientIdsOfActiveOrders.Add(client.Id);              
              break;
            }
          case OrderStatusEnum.Finished:
            {
              order.ClientId = client.Id;
              order.Client = client;
              break;
            }
          default:
            {
              break;
            }
        }

      }
    }


    public Task<bool> InsertModelsAsync()
    {
      throw new NotImplementedException();
    }

    public void LoadModels()
    {
      Models = JsonReader.GetModels().ToArray();
    }

    public async Task<bool> LoadDbModels()
    {
      try
      {
        using (var dbAccess = new WorkshopManagerContext())
        {
          Client[] clients = null;
          clients = dbAccess.Clients.ToArray();
          if (clients.Length == 0)
          {
            var insertSucceeded =  InsertModels();
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
          Models = clients;
          return true;
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return false;
      }
    }


    /// <summary>
    /// WARNING
    /// This method will not work correctly if Client rows have not re-seed'ed Id's... 
    /// Truncate or delete with reseed required if data is vanished.
    /// USE db-stored procedures to create/rebuild test data to avoid such troubles.
    /// </summary>
    /// <returns></returns>
    Client GetRandom()
    {
      var randomGen = new Random();
      long id = randomGen.Next(1, Models.Length);
      var client = Models.FirstOrDefault(m => m.Id == id);
      return client;
    }

    Client GetRandom<Client>()
    {
      throw new NotImplementedException();
    }
  }
}
