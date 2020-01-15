using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkshopManager.net.Utils;
using WorkshopManagerNET.Model;

namespace WorkshopManager.net.DataGenerator
{
  public interface IDataGenerator<T> where T : class
  {
    JsonModelsReader<T> JsonReader { get; set; }
    void LoadJSONModels();
    void LoadDbModels();
    T[] Models { get; set; }
    bool PersistModels(WorkshopManagerContext dbAccess = null);
    Task<bool> InsertModelsAsync();
  }
}
