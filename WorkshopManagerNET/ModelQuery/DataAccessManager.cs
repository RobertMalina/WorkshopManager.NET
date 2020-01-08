using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WorkshopManagerNET.Model;

namespace WorkshopManager.net.ModelQuery
{
  class DataAccessManager
  {
    private string GetDecision()
    {
      string decision = string.Empty;
      Console.Write("Are you sure? (Y/N) ");
      decision = Console.ReadLine();
      decision = decision.ToUpper();
      if (decision.Equals("Y") || decision.Equals("N"))
      {
        return decision;
      }
      else
      {
        Console.WriteLine("\nIncorrect confirmation input, operation skipped...");
        return string.Empty;
      }
    }
    public void Clear(string awareFlag = "")
    {
      if (awareFlag.Equals("-a") || awareFlag.Equals("-aware"))
      {
        TruncateAllData();
      }
      else
      {
        var decision = GetDecision();
        if (decision.Equals("Y"))
        {
          TruncateAllData();
        }
      }
    }
    private bool TruncateAllData()
    {
      try
      {
        int affectedRowsCount = 0;
        
        using(var dbAccess = new WorkshopManagerContext())
        {
          affectedRowsCount += dbAccess.Database.ExecuteSqlRaw("TRUNCATE TABLE [TimeLog]");
          affectedRowsCount += dbAccess.Database.ExecuteSqlRaw("TRUNCATE TABLE [Order]");
          affectedRowsCount += dbAccess.Database.ExecuteSqlRaw("TRUNCATE TABLE [OrderToWorker]");
          affectedRowsCount += dbAccess.Database.ExecuteSqlRaw("TRUNCATE TABLE [Client]");
          affectedRowsCount += dbAccess.Database.ExecuteSqlRaw("TRUNCATE TABLE [Worker]");
          affectedRowsCount += dbAccess.Database.ExecuteSqlRaw("TRUNCATE TABLE [Part]");
          affectedRowsCount += dbAccess.Database.ExecuteSqlRaw("TRUNCATE TABLE [Department]");

          affectedRowsCount += dbAccess.Database.ExecuteSqlRaw("TRUNCATE TABLE [AppUserToAppRole]");
          affectedRowsCount += dbAccess.Database.ExecuteSqlRaw("TRUNCATE TABLE [AppUser]");
          affectedRowsCount += dbAccess.Database.ExecuteSqlRaw("TRUNCATE TABLE [AppRole]");
        }
        Console.WriteLine($"All data has been truncated, {affectedRowsCount} rows affected");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return false;
      }
    }
  }
}
