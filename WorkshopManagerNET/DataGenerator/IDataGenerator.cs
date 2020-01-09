using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkshopManager.net.Utils;

namespace WorkshopManager.net.DataGenerator
{
  public interface IDataGenerator<T> where T : class
  {
    JsonModelsReader<T> JsonReader { get; set; }
    void LoadModels();
    T[] Models { get; set; }
    bool InsertModels();
    Task<bool> InsertModelsAsync();
  }
}
