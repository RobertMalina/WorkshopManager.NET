using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkshopManagerNET.Model;

namespace WorkshopManager.net.DataGenerator
{
  struct OrderQuartersOfDay
  {
    public OrderQuartersOfDay(int start, int end, int day)
    {
      QuarterStartIndex = start;
      QuarterEndIndex = end;
      OrderDay = day;
    }
    public int QuarterStartIndex;
    public int QuarterEndIndex;
    public int OrderDay;
  }
  struct MechanicianContribution
  {
    public MechanicianContribution(int workdayQuarterIndex, int workedQuartersCount, long orderId, long mechanicianId, int day)
    {
      WorkdayQuarterIndex = workdayQuarterIndex;
      WorkedQuartersCount = workedQuartersCount;
      OrderId = orderId;
      MechanicianId = mechanicianId;
      Day = day;
    }
    public int WorkdayQuarterIndex;
    public int WorkedQuartersCount;
    public int Day;
    public long OrderId;
    public long MechanicianId;
  }
  struct RandomContributionConfig
  {
    public RandomContributionConfig(int index, int duration)
    {
      QuarterIndex = index;
      Duration = duration;
    }
    public int QuarterIndex;
    public int Duration;
  }
  class TimeLogGenerator
  {
    /*
       0:00 - dayQuarter[0] (quarterIndex = 0),
       0:15 - dayQuarter[1] (quarterIndex = 1),
       0:30 - dayQuarter[2],
       0:45 - dayQuarter[3],
       1:00 - dayQuarter[4],
       ...
       7:00 - dayQuarter[28], (Workday starts, this is _workdayFirstQuarterIndex)
       7:15 - dayQuarter[29],
       7:30 - dayQuarter[30],
       7:45 - dayQuarter[31],
       8:00 - dayQuarter[32],
       ...
       16:45 - dayQuarter[67] (Workday ends, this is _workdayLastQuarterIndex)
       17:00 - dayQuarter[68] 
    */
    private const int _workdayFirstQuarterIndex = 28;
    private const int _workdayLastQuarterIndex = 67;

    public bool GenerateFor(Order[] orders)
    {
      OrderToWorker[] bindings;
      var timeLogs = new List<TimeLog>();
      orders = orders.Where(o => o.Status == OrderStatusEnum.Finished || o.Status == OrderStatusEnum.InProgress).ToArray();
      long[] orderIds = orders.Select(o => o.Id).ToArray();

      using (var dbAccess = new WorkshopManagerContext())
      {
        bindings = dbAccess.OrderToWorkers
          .Where(b => orderIds.Contains(b.OrderId)).ToArray();

        foreach (Order order in orders)
        {
          var orderBindings = bindings.Where(b => b.OrderId == order.Id).ToArray();
          var mechanicianIds = orderBindings.Select(b => b.WorkerId).ToArray();
          var quarters = GetAsOrderQuartersOfDay(order);
          var contributions = GenerateRandomContributions(quarters, mechanicianIds, order.Id);
          var timeLogsOfOrder = InstantiateTimeLogs(order, contributions);
          timeLogs.AddRange(timeLogsOfOrder);
        }
        dbAccess.BulkInsert<TimeLog>(timeLogs);
      }
      return true;
    }

    public OrderQuartersOfDay[] GetAsOrderQuartersOfDay(Order order)
    {
      var orderDurationQuarters = new List<OrderQuartersOfDay>();
      int days = 0;

      switch (order.Status)
      {
        case OrderStatusEnum.InProgress:
          {
            days = (DateTime.Now - order.DateStart.Value).Days;
            break;
          }
        case OrderStatusEnum.Finished:
          {
            days = (order.DateEnd.Value - order.DateStart.Value).Days;
            break;
          }
        default:
          {
            break;
          }
      }
      days = days + 1;

      int minutes,
        firstQuarterIndex,
        lastQuarterIndex;


      for (int i = 0; i < days; i++)
      {
        if (i == 0) //first day, so we need to check first quarter of order indexv(relative to whole day) 
        {
          minutes = order.DateStart.Value.Hour * 60 + order.DateStart.Value.Minute;
          firstQuarterIndex = minutes / 15;
          if (days == 1 && order.Status == OrderStatusEnum.Finished)
          {
            minutes = order.DateEnd.Value.Hour * 60 + order.DateEnd.Value.Minute;
            lastQuarterIndex = minutes / 15; //--> rare case when order was finished and started within the same day
          }
          else
          {
            lastQuarterIndex = _workdayLastQuarterIndex;
          }
          orderDurationQuarters.Add(new OrderQuartersOfDay()
          {
            OrderDay = i, //<- it will be 1st day of order duration (have number 0)
            QuarterStartIndex = firstQuarterIndex,
            QuarterEndIndex = lastQuarterIndex
          });
        }
        else if (i == days - 1) // last day
        {
          DateTime refDate;
          if (order.Status == OrderStatusEnum.Finished)
          {
            refDate = order.DateEnd.Value;
          }
          else
          {
            refDate = DateTime.Now;
          }
          minutes = refDate.Hour * 60 + refDate.Minute;
          lastQuarterIndex = minutes / 15;
          orderDurationQuarters.Add(new OrderQuartersOfDay()
          {
            OrderDay = i,
            QuarterStartIndex = _workdayFirstQuarterIndex,
            QuarterEndIndex = lastQuarterIndex
          });
        }
        else // middle day
        {
          orderDurationQuarters.Add(new OrderQuartersOfDay()
          {
            OrderDay = i,
            QuarterStartIndex = _workdayFirstQuarterIndex,
            QuarterEndIndex = _workdayLastQuarterIndex
          });
        }
      }
      return orderDurationQuarters.ToArray();
    }

    private RandomContributionConfig GenerateContributionConfig(int indexStart, int indexEnd, int maxDuration = 8)
    {
      var _rand = new Random();
      var logStartIndex = _rand.Next(indexStart, indexEnd); //NOT 'indexEnd -1' or 'indexEnd +1'
      var duration = _rand.Next(1, maxDuration+1);
      if (logStartIndex + duration > indexEnd)
      {
        duration = indexEnd - indexStart;
      }
      return new RandomContributionConfig(logStartIndex, duration);
    }
    public MechanicianContribution[] GenerateRandomContributions(OrderQuartersOfDay[] orderDuration, long[] mechanicianIds, long orderId)
    {
      int qtLogPeriod = 8;
      int passedQuarterIndex;
      bool doLog;
      var _rand = new Random();
      var contributions = new List<MechanicianContribution>();
      foreach (OrderQuartersOfDay orderQuarters in orderDuration)
      {      
        foreach (long mechanicianId in mechanicianIds)
        {
          passedQuarterIndex = orderQuarters.QuarterStartIndex;
          while (passedQuarterIndex < orderQuarters.QuarterEndIndex)
          {
            doLog = _rand.Next(1, 3) % 2 == 0;
            if (doLog)
            {
              var config = GenerateContributionConfig(passedQuarterIndex, passedQuarterIndex + qtLogPeriod);
              contributions.Add(new MechanicianContribution(config.QuarterIndex, config.Duration, orderId, mechanicianId, orderQuarters.OrderDay));
            }
            passedQuarterIndex += qtLogPeriod;
          }
        }
      }
      return contributions.ToArray();
    }

    public TimeLog[] InstantiateTimeLogs(Order order, MechanicianContribution[] contributions)
    {
      var timeLogs = new List<TimeLog>();

      foreach (MechanicianContribution contribution in contributions)
      {
        var blankDate = DateTime.MinValue;
        var refDate = order.DateStart.Value.AddDays(contribution.Day);
        var logTimeDays = (refDate - blankDate).Days;
        var dayQuartersCount = contribution.WorkdayQuarterIndex + contribution.WorkedQuartersCount;
        var minutes = dayQuartersCount * 15;
        var hours = minutes / 60.0;

        var logHours = (contribution.WorkedQuartersCount * 15) / 60.0;
        var logTime = blankDate
          .AddDays(logTimeDays)
          .AddHours(hours);

        timeLogs.Add(new TimeLog()
        {
          OrderId = order.Id,
          WorkerId = contribution.MechanicianId,
          LogTime = logTime,
          Hours = Convert.ToDecimal(logHours)
        });
      }
      return timeLogs.ToArray();
    }
  }
}
