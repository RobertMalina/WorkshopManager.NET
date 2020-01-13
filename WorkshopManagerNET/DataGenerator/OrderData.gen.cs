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
  class RandomHelper
  {
    private ClientData _clientsGenerator
    {
      get
      {
        return new ClientData();
      }
    }
    public void SetStatusesFor(Order[] orders)
    {
      var rand = new Random();
      foreach (Order order in orders)
      {       
        var oStatusRand = rand.Next(0, 10);
        if (oStatusRand < 2)
        {
          order.Status = OrderStatusEnum.Registered;
        }
        else if (oStatusRand < 5)
        {
          order.Status = OrderStatusEnum.InProgress;
        }
        else
        {
          order.Status = OrderStatusEnum.Finished;
        }
      }
    }

    public void MatchClientsWith(Order[] orders)
    {
      _clientsGenerator.MatchRandomlyWith(orders);
    }
  }

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

    public RandomHelper RandomSetup { get; set; }

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
          RandomSetup.MatchClientsWith(Models);
          RandomSetup.SetStatusesFor(Models);
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
