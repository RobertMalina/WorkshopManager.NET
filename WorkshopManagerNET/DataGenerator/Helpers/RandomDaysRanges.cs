using System;
using System.Collections.Generic;
using System.Text;

namespace WorkshopManager.net.DataGenerator.Helpers
{
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
      { return _rand.Next(_registerAgoRanges[0], _registerAgoRanges[1] + 1); }
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
        _registerAgoRangesClientHasActiveOrderNow[1] + 1);
      }
    }

  }
}
