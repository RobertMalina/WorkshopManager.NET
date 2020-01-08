using System;
using System.Collections.Generic;
using System.Text;
using WorkshopManagerNET.Model;

namespace WorkshopManager.net.Utils
{
  class WorkshopManagerCli
  {
    public void Run()
    {
      string cmd = string.Empty;
      while (!cmd.Equals("exit"))
      {
        Console.Write("WorkshopManagerCli> ");
        cmd = Console.ReadLine();
        switch (cmd)
        {
          case "list clients":
          case "l clients":
            {
              var clientsReader = new JsonModelsReader<Client>("clients.sample-data.json");
              var clients = clientsReader.GetModels();
              foreach (Client c in clients)
              {
                Console.WriteLine(c.FirstName);
              }
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
