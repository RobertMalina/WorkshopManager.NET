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
  class OrderRandomDurations
  {
    private int[] _lowComplexityRanges = new int[2] { 0, 2 };
    private int[] _mediumComplexityRanges = new int[2] { 1, 5 };
    private int[] _hightComplexityRanges = new int[2] { 3, 14 };
    private int[] _fromRegisterToStartRange = new int[2] { 4, 20 };

    public int FromRegisterToStart
    {
      get
      {
        return _rand.Next(
        _fromRegisterToStartRange[0],
        _fromRegisterToStartRange[1]+1);
      }
    }
    public int FromRegisterToStartMin
    {
      get
      {
        return _fromRegisterToStartRange[0];
      }
    }
    public int LowComplexityRandom
    {
      get
      {
        return _rand.Next(_lowComplexityRanges[0], _lowComplexityRanges[1]+1);
      }
    }
    public int LowComplexityMin
    {
      get
      {
        return _lowComplexityRanges[0];
      }
    }
    public int MediumComplexityRandom
    {
      get
      {
        return _rand.Next(_mediumComplexityRanges[0], _mediumComplexityRanges[1]+1);
      }
    }
    public int MediumComplexityMin
    {
      get
      {
        return _mediumComplexityRanges[0];
      }
    }
    public int HighComplexityMin
    {
      get
      {
        return _hightComplexityRanges[0];
      }
    }
    public int HighComplexityRandom
    {
      get
      {
        return _rand.Next(_hightComplexityRanges[0], _hightComplexityRanges[1]+1);
      }
    }

    /// <summary>
    /// Generowanie danych testowych. Służy do ustalenia daty rozpoczęcia (Order.DateStart) fikcyjnego zlecenia o statusie "obecnie trwającego".
    /// Zwraca (pseudo) losową ilość dni od ilu może trwać zlecenie o statusie "obecnie trwającego" (relatywnie do teraźniejszej daty).
    /// </summary>
    /// <param name="order"> Obiekt zlecenia którego wartość daty rozpoczęcia zostanie ustalona w wyniku działania tej metody.</param>
    /// <returns></returns>
    public void SetAsInProgress(Order order)
    {
      int[] targetRangesSet;

      switch (order.ComplexityClass)
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

      // rezultat może doprowadzić do uzyskania
      // daty wcześniejszej od daty zarejestrowania (zlecenie zaczęło się zanim zostało zarejestrowane - błąd chronologiczny)

      int orderDurationDays = _rand.Next(targetRangesSet[0], targetRangesSet[1] + 1) / 2;
      var startDate = DateTimeExt.PastSince(orderDurationDays);
      
      // zał:
      // RegisterDate < StartDate
      if (order.DateRegister > startDate)
      {
        //correct to early register date - move it back
        order.DateRegister = DateTimeExt.PastSince(orderDurationDays + FromRegisterToStartMin);
      }

      order.DateStart = startDate;
      order.DateRegister = order.DateRegister.SetCredibleTime();
      order.DateStart = order.DateStart.Value.SetCredibleTime();
    }

    public int When(DateTime registerDate)
    {
      int days = 0;
      int[] targetRangesSet = _fromRegisterToStartRange;

      return days;
    }

    Random _rand;
    public OrderRandomDurations()
    {
      _rand = new Random();
    }
  }
  class RandomDaysRanges
  {
    public OrderRandomDurations Durations { get; set; }

    Random _rand;
    public RandomDaysRanges()
    {
      _rand = new Random();
      Durations = new OrderRandomDurations();
    }

    private int[] _registerFreshAgoRanges = { 0, 14 };
    private int[] _registerAgoRanges = { 0, 90 };
    private int[] _registerAgoRangesClientHasActiveOrderNow = { 90, 180 };

    public int RegisterFreshAgo
    {
      get
      { return _rand.Next(_registerFreshAgoRanges[0], _registerFreshAgoRanges[1]); }
    }
    public int RegisterAgo
    {
      get
      { return _rand.Next(_registerAgoRanges[0], _registerAgoRanges[1]+1); }
    }

    /// <summary>
    /// Zwraca pseudo losową liczbę dni w ciągu których klient który obecnie posiada aktywne zlecenie
    /// (ma przypisane zlecenie z stanem Registered || InProgress).
    /// Wartość wygenerowanej liczby ma być większa od wartości wygenerowanej przez RegisterAgo.
    /// </summary>
    public int RegisterAgoClientHasActiveOrderNow
    {
      get
      {
        return _rand.Next(
        _registerAgoRangesClientHasActiveOrderNow[0],
        _registerAgoRangesClientHasActiveOrderNow[1]+1);
      }
    }

  }
  public static class DateTimeExt
  {
    /// <summary>
    /// Generuje losowy czas "relatywnie" do dnia roboczego - przyjęto że ów zaczyna się od 7:00 a kończy o 17:00.
    /// UWAGA: Zakłada że przekazana data ma ustawione wartości czasu jako 00:00:000.
    /// </summary>
    /// <returns></returns>
    public static DateTime SetCredibleTime(this DateTime dateTime)
    {
      var quartersCountMax = 10*60 / 15;
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
    public static DateTime PastSince( int daysAgo)
    {
      var result = DateTime.MinValue;
      var days = (DateTime.Now - DateTime.MinValue).Days - daysAgo;
      result = result.AddDays(days);
      return result;
    }
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
      order.DateRegister = DateTimeExt.PastSince(DaysRanges.RegisterFreshAgo)
        .SetCredibleTime();
    }

    public void SetForOrderInProgress(Order order)
    {
      order.DateRegister = DateTimeExt.PastSince(DaysRanges.RegisterFreshAgo)
        .SetCredibleTime();

      DaysRanges.Durations.SetAsInProgress(order);
    }
    private void AdjustTooEarlyRegisterDateOf(Order order)
    {
      order.DateRegister.AddDays(-DaysRanges.Durations.FromRegisterToStartMin);
    }
    private void MoveBackwardsOrderDates(Order order, int days)
    {
      order.DateRegister.AddDays(-days);
      order.DateStart.Value.AddDays(-days);
    }
    public void SetForFinishedOrder(Order order, bool clientHasActiveOrder = false)
    {
      // zał:
      // 1. registerDate < startDate < endDate <= DateTime.Now

      #region DateRegister setup
      // 2. (registerDate + daysToReceiveStartDate) < DateTime.Now      inaczej data rozpoczęcia będzie w przyszłości
      // 3. (startDate + daysToReceiveEndDate) < DateTime.Now
      if (clientHasActiveOrder)
      {
        order.DateRegister = DateTimeExt.PastSince(DaysRanges.RegisterAgoClientHasActiveOrderNow)
          .SetCredibleTime();
      }
      else
      {
        order.DateRegister = DateTimeExt.PastSince(DaysRanges.RegisterAgo)
          .SetCredibleTime();
      }
      #endregion

      #region DateStart setup

      int daysToReceiveStartDate = DaysRanges.Durations.FromRegisterToStart,
       nowVsRegisterDiff = (DateTime.Now - order.DateRegister).Days;

      if (daysToReceiveStartDate > nowVsRegisterDiff)
      {
        order.DateStart = order.DateRegister.SetCredibleTime();
        AdjustTooEarlyRegisterDateOf(order);
      }
      else
      {
        order.DateStart = order.DateRegister.AddDays(daysToReceiveStartDate).SetCredibleTime();
      }

      #endregion

      #region DateEnd setup

      int daysToReceiveEndDate = 0,
        nowVsStartDiff = (DateTime.Now - order.DateStart.Value).Days;

      //zał
      // DateEnd <= DateTime.Now
      // daysToReceiveEndDate <= daysSinceStart
      // DateTime.Now is latest possible EndDate

      switch (order.ComplexityClass)
      {
        case ComplexityClassEnum.Low:
          {
            daysToReceiveEndDate = DaysRanges.Durations.LowComplexityRandom;
            if (daysToReceiveEndDate > nowVsStartDiff)
            {
              if (DaysRanges.Durations.LowComplexityMin > nowVsStartDiff)
              {
                MoveBackwardsOrderDates(order, nowVsStartDiff);
                daysToReceiveEndDate = nowVsStartDiff;
              }
              else
              {
                daysToReceiveEndDate = DaysRanges.Durations.LowComplexityMin;
              }
            }
            break;
          }
        case ComplexityClassEnum.Medium:
          {
            daysToReceiveEndDate = DaysRanges.Durations.MediumComplexityRandom;
            if (daysToReceiveEndDate > nowVsStartDiff)
            {
              if (DaysRanges.Durations.MediumComplexityMin > nowVsStartDiff)
              {
                MoveBackwardsOrderDates(order, nowVsStartDiff);
                daysToReceiveEndDate = nowVsStartDiff;
              }
              else
              {
                daysToReceiveEndDate = DaysRanges.Durations.MediumComplexityMin;
              }
            }
            break;
          }
        case ComplexityClassEnum.High:
          {
            daysToReceiveEndDate = DaysRanges.Durations.HighComplexityRandom;
            if (daysToReceiveEndDate > nowVsStartDiff)
            {
              if (DaysRanges.Durations.HighComplexityMin > nowVsStartDiff)
              {
                MoveBackwardsOrderDates(order, nowVsStartDiff);
                daysToReceiveEndDate = nowVsStartDiff;
              }
              else
              {
                daysToReceiveEndDate = DaysRanges.Durations.HighComplexityMin;
              }
            }
            break;
          }
        default:
          {
            break;
          }
      }
      order.DateEnd = order.DateStart.Value.AddDays(daysToReceiveEndDate)
        .SetCredibleTime();
      #endregion
    }
  }
  class RandomOrderDataConfigurator
  {
    private CredibleDatetimeGenerator _credibleDatetimes;
    private ClientData _clientsGenerator;
    private MechanicianData _mechaniciansGenerator;
    private TimeLogGenerator _logGenerator;
    private PartGenerator _partsGenerator;

    public RandomOrderDataConfigurator()
    {
      _credibleDatetimes = new CredibleDatetimeGenerator();
      _clientsGenerator = new ClientData();
      _mechaniciansGenerator = new MechanicianData();
      _logGenerator = new TimeLogGenerator();
      _partsGenerator = new PartGenerator(new PartTestDataHelper());
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
              _credibleDatetimes.SetForOrderInProgress(order);
              break;
            }
          case OrderStatusEnum.Registered:
            {
              _credibleDatetimes.SetForRegisteredOrder(order);
              break;
            }
          case OrderStatusEnum.Finished:
            {
              _credibleDatetimes.SetForFinishedOrder(order);
              break;
            }
          default:
            {
              break;
            }
        }
      }
    }

    public void MatchWithMechanicians()
    {
      _mechaniciansGenerator.MatchRandomlyWithExistingOrders();
    }

    public void GenerateTimeLogsFor(Order[] orders)
    {
      _logGenerator.GenerateFor(orders);
    }

    public void GeneratePartsFor(Order[] orders)
    {
      _partsGenerator.GenerateFor(orders);
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
      try
      {
        Order[] orders = null;
        using (var dbAccess = new WorkshopManagerContext())
        {
          orders = dbAccess.Orders.ToArray();
        }
        if (orders.Length == 0)
        {
          PersistModels();
        }
        else
        {
          Models = orders;
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
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

      TestDataSetup.MatchWithMechanicians();
      TestDataSetup.GenerateTimeLogsFor(Models);
      TestDataSetup.GeneratePartsFor(Models);

      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine($"Test data has been embedded {DateTime.Now.ToString()}");
      Console.ForegroundColor = ConsoleColor.Gray;

      return true;
    }
  }
}

