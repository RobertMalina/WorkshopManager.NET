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
        cmd = Console.ReadLine();
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
            {
              var generator = new OrderData();
              foreach (Order o in generator.Models)
              {
                Console.WriteLine(o.ToString());
              }
              break;
            }
          case "insert orders":
          case "i orders":
            {
              var generator = new OrderData();
              generator.InsertModelsAsync();
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
          case "db c -a":
            {
              dataManager.Clear();
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
