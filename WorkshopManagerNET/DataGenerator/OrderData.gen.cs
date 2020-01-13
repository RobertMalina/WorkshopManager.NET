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
  class OrderInProgressDurations
  {
    public int[] _lowComplexityRanges = new int[2] { 0, 2 };
    private int[] _mediumComplexityRanges = new int[2] { 0, 5 };
    public int[] _hightComplexityRanges = new int[2] { 0, 14 };
    public int[] _fromRegisterToStartRange = new int[2] { 0, 14 };

    public int StartsSince(DateTime registerDate)
    {

      return 0;
    }
    public int When(ComplexityClassEnum complexity, DateTime refrenceDate, bool inProgress = false)
    {
      int days;
      int[] targetRangesSet;

      switch (complexity)
      {
        case ComplexityClassEnum.Low:
          {
            targetRangesSet = _lowComplexityRanges;
            break;
          }
        case ComplexityClassEnum.Medium:
          {
            targetRangesSet = _mediumComplexityRanges;
            break;
          }
        case ComplexityClassEnum.High:
          {
            targetRangesSet = _hightComplexityRanges;
            break;
          }
        default:
          {
            targetRangesSet = _lowComplexityRanges;
            break;
          }

      }
      int maxDaysAmount = 0;
      var daysSinceRefDate = (DateTime.Now - refrenceDate).Days;
      if (daysSinceRefDate < targetRangesSet[1])
      {
        maxDaysAmount = daysSinceRefDate;
      }
      else
      {
        maxDaysAmount = targetRangesSet[1];
      }

      days = _rand.Next(targetRangesSet[0], maxDaysAmount);

      if (inProgress)
      {
        return days / 2;
      }
      return days;
    }

    public int When(DateTime registerDate)
    {
      int days = 0;
      int[] targetRangesSet = _fromRegisterToStartRange;

      return days;
    }

    Random _rand;
    public OrderInProgressDurations()
    {
      _rand = new Random();
    }
  }


  class RandomDaysRanges
  {
    public OrderInProgressDurations Durations { get; set; }

    Random _rand;
    public RandomDaysRanges()
    {
      _rand = new Random();
      Durations = new OrderInProgressDurations();
    }

    public int RegisterFreshAgo { get { return _rand.Next(0, 14); } }
    public int RegisterOldAgo { get { return _rand.Next(0, 180); } }
  }
  class CredibleDatetimeGenerator
  {
    RandomDaysRanges DaysRanges { get; set; }
    public CredibleDatetimeGenerator()
    {
      DaysRanges = new RandomDaysRanges();
    }

    public void SetForRegisteredOrder(Order order)
    {
      var daysSinceRegistration = DaysRanges.RegisterFreshAgo;
      order.DateRegister = DateTime.Now.AddDays(-daysSinceRegistration).AddHours(8);
    }
    public void SetForOrderInProgress(Order order)
    {
      var daysSinceRegistration = DaysRanges.RegisterOldAgo;
      order.DateRegister = DateTime.Now.AddDays(-daysSinceRegistration).AddHours(8);
      switch (order.ComplexityClass)
      {
        case ComplexityClassEnum.Low:
          {
            order.DateStart = DateTime.Now.AddDays(-DaysRanges.Durations
              .When(ComplexityClassEnum.Low, order.DateRegister, true));
            break;
          }
        case ComplexityClassEnum.Medium:
          {
            order.DateStart = DateTime.Now.AddDays(-DaysRanges.Durations
              .When(ComplexityClassEnum.Medium, order.DateRegister, true));
            break;
          }
        case ComplexityClassEnum.High:
          {
            order.DateStart = DateTime.Now.AddDays(-DaysRanges.Durations
              .When(ComplexityClassEnum.High, order.DateRegister, true));
            break;
          }
        default:
          {
            break;
          }
      }
    }
    public void SetForFinishedOrder(Order order, bool clientHasActiveOrder = false)
    {
      var daysSinceRegistration = DaysRanges.RegisterOldAgo;
      order.DateRegister = DateTime.Now.AddDays(-daysSinceRegistration).AddHours(8);
      order.DateStart = order.DateRegister.AddDays(
        -DaysRanges.Durations.StartsSince(order.DateRegister)).AddHours(8);
      
      switch (order.ComplexityClass)
      {
        case ComplexityClassEnum.Low:
          {
            order.
            break;
          }
        case ComplexityClassEnum.Medium:
          {
            break;
          }
        case ComplexityClassEnum.High:
          {
            break;
          }
        default:
          {
            break;
          }
      }
    }
  }

  class RandomHelper
  {
    public CredibleDatetimeGenerator CredibleDatetimes { get; set; }
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

    public void SetCredibleDateTimesFor(Order[] orders)
    {
      foreach (Order order in orders)
      {
        switch (order.Status)
        {
          case OrderStatusEnum.InProgress:
            {
              CredibleDatetimes.SetForOrderInProgress(order);
              break;
            }
          case OrderStatusEnum.Registered:
            {
              CredibleDatetimes.SetForRegisteredOrder(order);
              break;
            }
          case OrderStatusEnum.Finished:
            {
              CredibleDatetimes.SetForFinishedOrder(order);
              break;
            }
          default:
            {
              break;
            }
        }
      }
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

    public RandomHelper RandomDataSetup { get; set; }

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
        RandomDataSetup.MatchClientsWith(Models);
        RandomDataSetup.SetStatusesFor(Models);
        RandomDataSetup.SetCredibleDateTimesFor(Models);

        using (var dbAccess = new WorkshopManagerContext())
        {
          dbAccess.BulkInsert(Models);
          dbAccess.SaveChanges();
          return true;
        }
      }
      catch (Exception e)
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
