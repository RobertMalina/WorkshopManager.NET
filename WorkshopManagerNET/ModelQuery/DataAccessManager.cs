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
    public void Clear(string cmd)
    {
      if (cmd.Contains(" -aware") || cmd.Contains(" -a"))
      {
        TruncateAllData();
      }
      else
      {
        var decision = GetDecision();
        if (decision.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
        {
          TruncateAllData();
        }
      }
    }
    private bool TruncateAllData()
    {
      try
      {
        using (var dbAccess = new WorkshopManagerContext())
        {
          dbAccess.Database.ExecuteSqlRaw("EXEC [dbo].[ClearDatabase] @restoreRoles = 1");
        }
        Console.WriteLine("All database data has been dropped...");
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
