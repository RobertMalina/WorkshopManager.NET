using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkshopManagerNET.Model;

namespace WorkshopManager.net.ModelQuery
{
  class OrderService
  {
    public void OrdersMultiqueryTest()
    {
      try
      {
        using (var dbAccess = new WorkshopManagerContext())
        {
          var order = dbAccess.Orders
            .Include("Client")
            .Include("WorkerOrders")
            .Where(o => o.Client.PhoneNumber.Equals("212992581"))
            .SingleOrDefault();
          if (order != null)
          {
            Console.WriteLine(order.Client.LastName);

            var engagedWorkersIds = order.WorkerOrders
                .Select(wo => wo.WorkerId)
                .ToArray();

            var engagedWorkers =
              dbAccess.Mechanicians.Where(w =>
                engagedWorkersIds.Contains(w.Id)).ToArray();

            if (engagedWorkers.Count() > 0)
            {
              foreach (var worker in engagedWorkers)
                Console.WriteLine($"{worker.FirstName} {worker.LastName}");
            }
            else
            {
              Console.WriteLine("Not found any assigned workers to order...");
            }
          }
          else
            Console.WriteLine("Order not found...");
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
    }
  }
}
