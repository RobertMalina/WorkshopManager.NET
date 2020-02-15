using System;
using System.Collections.Generic;
using System.Text;
using WorkshopManagerNET.Model;

namespace WorkshopManager.net.DataGenerator.Helpers
{
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
}
