using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopManager.net.DataGenerator.Helpers;
using WorkshopManager.net.ModelQuery;
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

  public static class DateTimeExt
  {
    /// <summary>
    /// Generuje losowy czas "relatywnie" do dnia roboczego - przyjęto że ów zaczyna się od 7:00 a kończy o 17:00.
    /// UWAGA: Zakłada że przekazana data ma ustawione wartości czasu jako 00:00:000.
    /// </summary>
    /// <returns></returns>
    public static DateTime SetCredibleTime(this DateTime dateTime)
    {
      var quartersCountMax = 10 * 60 / 15;
      var quartersCount = new Random().Next(1, quartersCountMax);
      var days = (dateTime - DateTime.MinValue).Days;
      var idleTimeMinutes = 7 * 4 * 15; //7 hours * 4 quarters * 15 minutes
      var minutes = idleTimeMinutes + (quartersCount * 15);

      var resultDate = DateTime.MinValue.AddDays(days).AddMinutes(minutes);
      return resultDate;
    }

    /// <summary>
    /// Zwraca datę z przeszłości odległą względem teraźniejszej o zadaną ilość dni.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTime PastSince(int daysAgo)
    {
      var result = DateTime.MinValue;
      var days = (DateTime.Now - DateTime.MinValue).Days - daysAgo;
      result = result.AddDays(days);
      return result;
    }
  }

  class OrderCase
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public string VehicleDescription { get; set; }
    public ComplexityClassEnum Complexity { get; set; }
    public string[] PartCodes { get; set; }
  }
  class OrderData : IDataGenerator<Order>
  {
    private JsonModelsReader<Order> _reader = null;
    private JsonModelsReader<OrderCase> _casesReader = new JsonModelsReader<OrderCase>(_orderCasesjsonFileName);

    private Order[] _models = null;
    private const string _jsonFileName = "orders.sample-data.json";
    private const string _orderCasesjsonFileName = "order-cases.json";

    public OrderData()
    {
      TestDataSetup = new RandomOrderDataConfigurator();
    }

    public RandomOrderDataConfigurator TestDataSetup { get; set; }

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
          LoadJSONModels();
        }
        return _models;
      }
      set => _models = value;
    }
    public async Task<bool> InsertModelsAsync()
    {
      throw new NotImplementedException();
    }

    public void LoadJSONModels()
    {
      var sampleCases = _casesReader.GetModels().ToArray();
      var orders = new List<Order>();
      foreach (OrderCase orderCase in sampleCases)
        orders.Add(new Order()
        {
          Title = orderCase.Title,
          VehicleDescription = orderCase.VehicleDescription,
          DateRegister = DateTime.MinValue,
          ComplexityClass = orderCase.Complexity,
          Description = orderCase.Description
        });

      Models = orders.ToArray();
    }

    public void LoadDbModels()
    {
      Order[] orders = null;
      using (var dbAccess = new WorkshopManagerContext())
      {
        orders = dbAccess.Orders.ToArray();
      }

      if (orders.Length == 0)
        PersistModels();

      else
        Models = orders;
    }

    public void InsertModelsAndRelatedData()
    {
      try
      {
        Console.WriteLine("Test data is about to be inserted to development database...");
        PersistModels();
        TestDataSetup.MatchWithMechanicians();
        TestDataSetup.GenerateTimeLogsFor(Models);
        TestDataSetup.GeneratePartsFor(Models);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Test data has been embedded {DateTime.Now.ToString()}");
        Console.ForegroundColor = ConsoleColor.Gray;
      }
      catch(Exception e)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(e.Message);
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("Test data insert failed, so database-clear is performed in aim to avoid inconsistency errors.");

        var dataManager = new DataAccessManager();
        dataManager.Clear("db clear -aware");

      }
    }

    public bool PersistModels(WorkshopManagerContext dbAccess = null)
    {
      TestDataSetup.SetStatusesFor(Models);
      TestDataSetup.MatchClientsWith(Models);
      TestDataSetup.SetCredibleDateTimesFor(Models);

      using (dbAccess = new WorkshopManagerContext())
      {
        dbAccess.BulkInsert(Models);
        dbAccess.SaveChanges();
        Models = dbAccess.Orders.ToArray();
      }

      return true;
    }
  }
}