using System;
using System.Collections.Generic;
using System.Text;
using WorkshopManager.net.Utils;
using WorkshopManagerNET.Model;

namespace WorkshopManager.net.DataGenerator
{
  class ClientData : IDataGenerator<Client>
  {
    private const string _jsonSrcPath = @"..\..\..\DataGenerator\sources\clients.sample-data.json";

    private JsonModelsReader<Client> _reader;
    public JsonModelsReader<Client> JsonReader { get { return _reader; } set { _reader = new JsonModelsReader<Client>(_jsonSrcPath);  } }

    public bool InsertDefaultFromJson()
    {
      throw new NotImplementedException();
    }
  }
}
