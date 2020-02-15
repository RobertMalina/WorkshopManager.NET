using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using WorkshopManagerNET.Model;

namespace WorkshopManager.net.Utils
{
  class NodeServerClient
  {
    private const string _loginEndpointAddr = "http://localhost:4210/api/auth/receive/bcrypted";
    private const string _passwordHash = "$2b$12$yerVSct1CoH0y6PIa5pt9.asj7GN2RRgjhkXPU/G0WtYloo5OSdpa";
    private const string _username = "workshopManagerCli";
    private const string _password = "zaq12wsx";

    private AppUser _user;

    private AppUserHelper _appUserHelper;

    NodeServerClient()
    {
      _user = new AppUser() { Username = _username, PasswordHash = _passwordHash };
      LoadUser();
    }

    public bool isLoggedIn()
    {
      return _user.Token != null;
    }

    private AppUser PersistUser(WorkshopManagerContext db)
    {
      db.Users.Add(_user);
      string[] roleNames = Enum.GetValues(typeof(AppRoleEnum))
          .Cast<string>()
          .Select(x => x.ToString())
          .ToArray();

      var roles = db.Roles.Where(r => roleNames.Contains(r.Name)).ToArray();

      if (roles.Length == 0)
      {
        throw new Exception($"could not found any roles - verify AppRoleEnum values...");
      }

      foreach (AppRole role in roles)
      {
        _user.Roles.Add(new AppUserToAppRole() { UserId = _user.Id, RoleId = role.Id });
      }

      db.SaveChanges();

      return db.Users.SingleOrDefault(u => u.Username.Equals(_username));
    }

    private bool LoadUser()
    {
      try
      {
        using(var db = new WorkshopManagerContext())
        {
          var user = db.Users.SingleOrDefault(u => u.Username.Equals(_username));
          if(user != null)
          {
            _user = user;
          }
          else
          {
            _user = PersistUser(db);
          }
        }
      }
      catch (Exception e)
      {
        return false;
      }
      return true;
    }


    public bool Login()
    {
      try
      {
        var body = new Dictionary<string, string>();
        body.Add("username", _username);
        body.Add("password", _password);
        var httpModule = new HttpModule();
        var loggedUser = httpModule.Post<AppUser>(_loginEndpointAddr, body).Result;
        if(loggedUser == null)
        {
          throw new Exception("Node server login trial failed.");
        }
        else
        {
          _user = loggedUser;
        }
      }
      catch(Exception e)
      {
        return false;
      }
      return true;
    }

  }
}
