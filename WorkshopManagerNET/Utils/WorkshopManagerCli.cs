using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkshopManager.net.DataGenerator;
using WorkshopManager.net.ModelQuery;
using WorkshopManagerNET.Model;

namespace WorkshopManager.net.Utils
{
  class WorkshopManagerCli
  {
    public void Run()
    {
      var dataManager = new DataAccessManager();
      string cmd = string.Empty;
      while (!cmd.Equals("exit"))
      {
        Console.Write("WorkshopManagerCli> ");
        cmd = Console.ReadLine().ToLower();
        switch (cmd)
        {
          case "": // enter
            {
              break;
            }
          case "list clients":
          case "l clients":
            {
              var generator = new ClientData();            
              foreach (Client c in generator.Models)
              {
                Console.WriteLine(c.ToString());
              }
              break;
            }
          case "list orders":
          case "l orders":
          case "l orders -r":
            {
              if (cmd.Contains("-r"))
              {
                Console.Write("how many days ago? ");
                int days;
                var read = Console.ReadLine();
                if(Int32.TryParse(read, out days))
                {
                  using (var db = new WorkshopManagerContext())
                  {
                    var res = db.Orders.FromSqlRaw<Order>($"SELECT * FROM [dbo].[GetOrdersRegisteredSince]({days})");
                    foreach(Order o in res)
                    {
                      Console.WriteLine(o.ToString());
                    }
                  }
                }
                else
                {
                  Console.WriteLine("Incorrect days value passed...");
                }
              }
              else
              {
                var generator = new OrderData();
                foreach (Order o in generator.Models)
                {
                  Console.WriteLine(o.ToString());
                }
              }

              break;
            }
          case "insert orders":
          case "i orders":
            {
              var generator = new OrderData();
              generator.LoadDbModels();
              break;
            }
          case "list mechanicians":
          case "l mechanicians":
          case "l mechs":
            {
              var generator = new MechanicianData();
              foreach (Mechanician m in generator.Models)
              {
                Console.WriteLine(m.ToString());
              }
              break;
            }
          case "insert mechanicians":
          case "i mechanicians":
            {
              var generator = new MechanicianData();
              generator.InsertModelsAsync();
              break;
            }
          case "db clear":
          case "db clear -aware":
          case "db clear -a":
          case "db c -a":
            {

              dataManager.Clear(cmd);
              break;
            }
          case "db reset":
          case "db reset -aware":
          case "db reset -a":
          case "db r -a":
            {
              dataManager.Clear(cmd);
              var generator = new OrderData();
              generator.LoadDbModels();
              break;
            }
          case "exit":
          case "q":
            {
              if (cmd.Equals("q"))
              {
                cmd = "exit";
              }              
              break;
            }
          default:
            {
              Console.WriteLine($"\n{cmd} is not known WorkshopManagerCli command...\n");             
              break;
            }
        }
      }
      Console.WriteLine("WorkshopManagerCli exited, press any key to close terminal window...");
    }
  }
}
