using System;
using System.Collections.Generic;
using System.Text;
using WorkshopManagerNET.Model;

namespace WorkshopManager.net.DataGenerator.Helpers
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
        _fromRegisterToStartRange[1] + 1);
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
        return _rand.Next(_lowComplexityRanges[0], _lowComplexityRanges[1] + 1);
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
        return _rand.Next(_mediumComplexityRanges[0], _mediumComplexityRanges[1] + 1);
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
        return _rand.Next(_hightComplexityRanges[0], _hightComplexityRanges[1] + 1);
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
}
