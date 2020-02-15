using System;
using System.Collections.Generic;
using System.Text;
using WorkshopManagerNET.Model;

namespace WorkshopManager.net.DataGenerator.Helpers
{
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
}
