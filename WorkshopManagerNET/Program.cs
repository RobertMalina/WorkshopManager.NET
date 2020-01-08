using WorkshopManagerNET.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WorkshopManager.net.Utils;

namespace WorkshopManagerNET
{
  class Program
  {
    static void Main(string[] args)
    {
      var cli = new WorkshopManagerCli();
      cli.Run();
    }
  }
}
