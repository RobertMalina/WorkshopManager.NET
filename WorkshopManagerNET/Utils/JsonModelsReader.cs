using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WorkshopManager.net.Utils
{
  public class JsonModelsReader<T> where T : class
  {
    private bool _srcFileExists = false;
    private const string _baseDirPath = @"..\..\..\DataGenerator\sources\";
    private string _srcPath = string.Empty;
    public JsonModelsReader(string fileName)
    {
      _srcPath = $"{_baseDirPath}{fileName}";
      if (!DoISeeTheFile())
      {
        Console.WriteLine(Environment.CurrentDirectory);
        Console.WriteLine($"File with path: {_srcPath} not found...");
      }
      else
      {
        _srcFileExists = true;
      }
    }

    private bool DoISeeTheFile()
    {
      return File.Exists(_srcPath);
    }
    public List<T> GetModels()
    {
      if (!_srcFileExists)
      {
        return new List<T>();
      }
      try
      {
        using (StreamReader reader = new StreamReader(_srcPath))
        {
          string json = reader.ReadToEnd();
         return JsonConvert.DeserializeObject<List<T>>(json);
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return new List<T>();
    }
  }
}
