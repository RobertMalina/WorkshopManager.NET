using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkshopManagerNET.Model;

namespace WorkshopManagerNET.Model
{
  public enum AppRoleEnum { regular, supervisor, administrator, mechanician }

  partial class AppUser
  {
    public string Token { get; set; }
    public string Password { get; set; }
    public static bool IsValidToInsert(AppUser appUser)
    {
      if (appUser == null)
        return false;

      return
        appUser.PasswordHash != null &&
        appUser.Username != null;
    }
  }
}

namespace WorkshopManager.net.Utils
{
  class AppUserHelper
  {
    private AppRole[] _roles;

    public bool HandleCliCommand(string originalCmd)
    {
      try
      {
        string cmd = string.Empty;
        bool result = true;

        if (originalCmd.Contains("set-role"))
        {
          cmd = "set-role";
        }

        switch (cmd)
        {
          case "set-role":
            {
              //command:  user-helper set-role <username> <rolename>
              string[] parameters = originalCmd.Split(" ");
              var userName = parameters[2];
              var roleName = parameters[3];

              var user = FindAppUser(userName);
              if (user == null)
              {
                throw new Exception($"User with username: {userName} could not be found...");
              }
              var roleEnum = Enum.Parse(typeof (AppRoleEnum), roleName);
              if(roleName == null)
              {
                throw new Exception($"Role name: {roleName} does not match to any known role name...");
              }

              result = AssignRoles(user, new AppRoleEnum[] { (AppRoleEnum)roleEnum });

              break;
            }
          default:
            {
              Console.WriteLine($"\n{cmd} is not known WorkshopManagerCli command...\n");
              result = false;
              break;
            }
        }
        return result;
      }
      catch(Exception e)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(e.Message);
        Console.ForegroundColor = ConsoleColor.Gray;
        return false;
      }

    }
    public bool AssignRoles(AppUser user, AppRoleEnum[] roles, WorkshopManagerContext dbContext = null)
    {
      try
      {
        bool useOwnDbContext = dbContext == null;
        if (useOwnDbContext)
        {
          dbContext = new WorkshopManagerContext();
        }

        if (_roles == null)
        {
          LoadRoles(dbContext);
        }

        string[] roleNames = roles.Select(r => r.ToString()).ToArray();

        var foundAppRoles = _roles
          .Where(r => roleNames.Contains(r.Name))
          .ToArray();

        if(roleNames.Length != foundAppRoles.Length)
        {
          string[] notFoundRoleNames = roleNames
            .Where(rn => !foundAppRoles.Select(r => r.Name).ToArray()
            .Contains(rn)).ToArray();
          throw new Exception(
            $"Roles with names: ${ string.Join(",", notFoundRoleNames) } " +
            $"could not be found in database - verify AppRoleEnum values.");
        }

        foreach (AppRole role in foundAppRoles)
        {
          user.Roles.Add(new AppUserToAppRole() { UserId = user.Id, RoleId = role.Id });
        }

        dbContext.SaveChanges();

        if (useOwnDbContext)
        {
          dbContext.Dispose();
        }

        return true;
      }
      catch(Exception e)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(e);
        Console.ForegroundColor = ConsoleColor.Gray;
        return false;
      }
    }
    private void LoadRoles(WorkshopManagerContext dbAccess)
    {
      _roles = dbAccess.Roles.ToArray();
    }

    private AppUser FindAppUser(string username)
    {
      using(var db = new WorkshopManagerContext())
      {
        return db.Users.SingleOrDefault(u => u.Username.Equals(username));
      }
    }
  }
}
