using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopManager.net.Utils;
using WorkshopManagerNET.Model;

namespace WorkshopManagerNET.Model
{
  partial class Worker
  {
    public override string ToString()
    {
      return $"{FirstName} {LastName} {PhoneNumber}";
    }
    public string UserName { get { return $"{FirstName.ToLower()}_{LastName.ToLower()}"; } }
  }
}


namespace WorkshopManager.net.DataGenerator
{
  
  class MechanicianData : IDataGenerator<Mechanician>
  {
    // TODO do zmiany
    private const string bcryptServiceAddr = "http://localhost:4210/api/auth/receive/bcrypted";
    private const string _jsonFileName = "mechanicians.sample-data.json";

    private JsonModelsReader<Mechanician> _reader = null;
    private Mechanician[] _models = null;
    public JsonModelsReader<Mechanician> JsonReader
    {
      get
      {
        if (_reader == null)
        {
          _reader = new JsonModelsReader<Mechanician>(_jsonFileName);
        }
        return _reader;
      }
      set => _reader = value;
    }

    public Mechanician[] Models
    {
      get
      {
        if (_models == null)
        {
          LoadModels();
        }
        return _models;
      }
      set => _models = value;
    }

    public bool InsertModels()
    {
      throw new NotImplementedException();
    }

    public async Task<bool> InsertModelsAsync()
    {
      try
      {
        var httpModule = new HttpModule();

        using (var dbAccess = new WorkshopManagerContext())
        {
          List<AppUser> authUsers = new List<AppUser>();
          
          foreach(Mechanician m in Models)
          {
            AppUser authUser = null;
            var body = new Dictionary<string, string>();
            body.Add("Username", m.UserName);
            body.Add("Password", "zaq12wsx");
            authUser = await httpModule.Post<AppUser>(bcryptServiceAddr, body);
            if(authUser != null)
            {
              authUsers.Add(authUser);
            }
          }
          await dbAccess.BulkInsertAsync(authUsers);
          dbAccess.SaveChanges();

          var usernames = Models.Select(m => m.UserName).ToArray();
          authUsers = dbAccess.Users.Where(u => usernames.Contains(u.Username)).ToList();

          foreach (Mechanician m in Models)
          {
            var authUser = authUsers.FirstOrDefault(u => u.Username.Equals(m.UserName));
            if(authUser != null)
            {
              m.AppUserId = authUser.Id;
              m.AppUser = authUser;
            }
          }

          var validModels = Models.Where(m => m.AppUser != null).ToArray();
          if(validModels.Length > 0)
          {
            await dbAccess.BulkInsertAsync(validModels);
            dbAccess.SaveChanges();
          }
        }
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return false;
      }
    }

    public void LoadModels()
    {
      Models = JsonReader.GetModels().ToArray();
    }
  }
}
