using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
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

    public async Task<bool> InsertModels()
    {
      try
      {
        using(var dbAccess = new WorkshopManagerContext())
        {
          await dbAccess.BulkInsertAsync(Models);
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

    public Task<bool> InsertModelsAsync()
    {
      throw new NotImplementedException();
    }

    public void LoadModels()
    {
      Models = JsonReader.GetModels().ToArray();
    }

    bool IDataGenerator<Client>.InsertModels()
    {
      throw new NotImplementedException();
    }
  }
}
