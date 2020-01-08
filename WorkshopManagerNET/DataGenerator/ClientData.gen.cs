using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using WorkshopManager.net.Utils;
using WorkshopManagerNET.Model;

namespace WorkshopManager.net.DataGenerator
{
  class ClientData
  {
    private const string _jsonSrcPath = @"..\..\..\DataGenerator\sources\clients.sample-data.json";

    private JsonModelsReader<Client> _reader = null;
    private Client[] _models = null;
    public JsonModelsReader<Client> JsonReader
    {
      get
      {
        if (_reader == null)
        {
          _reader = new JsonModelsReader<Client>(_jsonSrcPath);
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
          dbAccess.BulkInsert(Models);
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

    public void LoadModels()
    {
      var clientsReader = new JsonModelsReader<Client>("clients.sample-data.json");
      Models = clientsReader.GetModels().ToArray();
    }
  }
}
