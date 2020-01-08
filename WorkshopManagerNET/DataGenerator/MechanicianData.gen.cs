using System;
using System.Collections.Generic;
using System.Text;
using WorkshopManagerNET.Model;

namespace WorkshopManager.net.DataGenerator
{
  
  class MechanicianData : IDataGenerator<Mechanician>
  {
    private const string _jsonSrcPath = @"..\..\..\DataGenerator\sources\mechanicians.sample-data.json";

    private JsonModelsReader<Mechanician> _reader = null;
    private Mechanician[] _models = null;
    public JsonModelsReader<Mechanician> JsonReader
    {
      get
      {
        if (_reader == null)
        {
          _reader = new JsonModelsReader<Mechanician>(_jsonSrcPath);
        }
        return _reader;
      }
      set => _reader = value;
    }

    public Mechanician[] Models
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
        using (var dbAccess = new WorkshopManagerContext())
        {
          dbAccess.BulkInsert(Models);
          dbAccess.SaveChanges();
        }
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return false;
      }
    }

    public void LoadModels()
    {
      var MechaniciansReader = new JsonModelsReader<Mechanician>("Mechanicians.sample-data.json");
      Models = MechaniciansReader.GetModels().ToArray();
    }
  }
}
