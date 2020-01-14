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
    private int[] _mediumComplexityRanges = new int[2] { 0, 5 };
    private int[] _hightComplexityRanges = new int[2] { 0, 14 };
    private int[] _fromRegisterToStartRange = new int[2] { 4, 20 };

    public int FromRegisterToStart
    {
      get
      {
        return _rand.Next(
        _fromRegisterToStartRange[0],
        _fromRegisterToStartRange[1]);
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
        return _rand.Next(_lowComplexityRanges[0], _lowComplexityRanges[1]);
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
        return _rand.Next(_mediumComplexityRanges[0], _mediumComplexityRanges[1]);
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
        return _rand.Next(_hightComplexityRanges[0], _hightComplexityRanges[1]);
      }
    }

    /// <summary>
    /// Zwraca losowo wygenerowaną, wiarygodną liczbę dni jaka mogła upłynąć od momentu rozpoczęcia zlecenia
    /// do dnia obecnego (DateTime.Now). Zwrócona liczba po odjęciu od daty rejestracji (AddDays(-value)) powinna 
    /// prowadzić do uzyskania daty chronologicznie późniejszej od daty rejestracji. 
    /// </summary>
    /// <param name="complexity"> stopień złożoności zlecenia - na jego podstawie, wygenerowana zostanie losowo liczba dni jaka upłynęła do momentu zaangażowania mechaników w realizację.</param>
    /// <param name="refrenceDate"> data chronologicznie wcześniejsza od daty jaką uzyska się po odjęciu wyniku działania metody od obecnej daty</param>
    /// <returns></returns>
    public int ForInProgress(ComplexityClassEnum complexity, DateTime refrenceDate)
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

      return days / 2;
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
      { return _rand.Next(_registerAgoRanges[0], _registerAgoRanges[1]); }
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
        _registerAgoRangesClientHasActiveOrderNow[1]);
      }
    }

  }

  public static class DateTimeExt
  {
    /// <summary>
    /// Generuje losowy czas "relatywnie" do dnia roboczego - przyjęto że ów zaczyna się od 7:00 a kończy o 17:00.
    /// UWAGA: Zakłada że przekazana data ma ustawione wartości czasu jako 00:00:000.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTime AddTimeRelativeToWorkday(this DateTime dateTime)
    {
      var quartersCountMax = new TimeSpan(10, 0, 0).Minutes / 15;
      var quartersCount = new Random().Next(1, quartersCountMax);
      var relTimespan = new TimeSpan(0, 8, quartersCount * 15, DateTime.Now.Second, DateTime.Now.Millisecond);
      dateTime.AddHours(relTimespan.Hours);
      dateTime.AddMinutes(relTimespan.Minutes);
      dateTime.AddSeconds(relTimespan.Seconds);
      dateTime.AddMilliseconds(relTimespan.Milliseconds);
      return dateTime;
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
      var daysSinceRegistration = DaysRanges.RegisterFreshAgo;
      order.DateRegister = DateTime.Now.AddDays(-daysSinceRegistration).AddTimeRelativeToWorkday();
    }
    public void SetForOrderInProgress(Order order)
    {
      var daysSinceRegistration = DaysRanges.RegisterFreshAgo;
      order.DateRegister = DateTime.Now.AddDays(-daysSinceRegistration)
        .AddTimeRelativeToWorkday();
      switch (order.ComplexityClass)
      {
        case ComplexityClassEnum.Low:
          {
            order.DateStart = DateTime.Now.AddDays(-DaysRanges.Durations
              .ForInProgress(ComplexityClassEnum.Low, order.DateRegister))
              .AddTimeRelativeToWorkday();
            break;
          }
        case ComplexityClassEnum.Medium:
          {
            order.DateStart = DateTime.Now.AddDays(-DaysRanges.Durations
              .ForInProgress(ComplexityClassEnum.Medium, order.DateRegister))
              .AddTimeRelativeToWorkday();
            break;
          }
        case ComplexityClassEnum.High:
          {
            order.DateStart = DateTime.Now.AddDays(-DaysRanges.Durations
              .ForInProgress(ComplexityClassEnum.High, order.DateRegister))
              .AddTimeRelativeToWorkday();
            break;
          }
        default:
          {
            break;
          }
      }
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
        order.DateRegister = DateTime.Now.AddDays(-DaysRanges.RegisterAgoClientHasActiveOrderNow)
          .AddTimeRelativeToWorkday();
      }
      else
      {
        order.DateRegister = DateTime.Now.AddDays(-DaysRanges.RegisterAgo)
          .AddTimeRelativeToWorkday();
      }
      #endregion

      #region DateStart setup

      int daysToReceiveStartDate = DaysRanges.Durations.FromRegisterToStart,
       nowVsRegisterDiff = (DateTime.Now - order.DateRegister).Days;

      if (daysToReceiveStartDate > nowVsRegisterDiff)
      {
        order.DateStart = order.DateRegister;
        AdjustTooEarlyRegisterDateOf(order);
      }
      else
      {
        order.DateStart = order.DateRegister.AddDays(daysToReceiveStartDate);
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
        .AddTimeRelativeToWorkday();
      #endregion
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
