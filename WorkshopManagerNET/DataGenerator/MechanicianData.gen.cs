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
  public enum AppRoleEnum { regular, supervisor, administrator, mechanician }
  partial class Worker
  {
    public override string ToString()
    {
      return $"{FirstName} {LastName} {PhoneNumber}";
    }
    public string UserName { get { return $"{FirstName.ToLower()}_{LastName.ToLower()}"; } }
  }

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


namespace WorkshopManager.net.DataGenerator
{
  class MechanicianData : IDataGenerator<Mechanician>
  {
    // TODO do zmiany
    private const string bcryptServiceAddr = "http://localhost:4210/api/auth/receive/bcrypted";
    private const string _jsonFileName = "mechanicians.sample-data.json";
    private const int _maxMechanciansPerOrder = 3;

    private JsonModelsReader<Mechanician> _reader = null;
    private Mechanician[] _models = null;
    private AppUserHelper _appUserHelper;
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
          LoadJSONModels();
        }
        return _models;
      }
      set => _models = value;
    }

    private void PersistSystemUsers(WorkshopManagerContext dbAccess, Mechanician[] mechanicians)
    {
      var httpModule = new HttpModule();
      List<AppUser> authUsers = new List<AppUser>();
      foreach (Mechanician m in Models)
      {
        AppUser authUser = null;
        var body = new Dictionary<string, string>();
        body.Add("username", m.UserName);
        body.Add("password", "zaq12wsx");
        authUser = httpModule.Post<AppUser>(bcryptServiceAddr, body).Result;
        if (AppUser.IsValidToInsert(authUser))
        {
          authUsers.Add(authUser);
        }
        else
        {
          throw new Exception("Bcrypt remote server returned invalid AppUser data...");
        }
      }
      dbAccess.BulkInsert<AppUser>(authUsers);
      dbAccess.SaveChanges();
    }

    private void AssignBasicRoles(
      AppUser[] authUsers,
      WorkshopManagerContext dbAccess,
      AppRoleEnum basicRole = AppRoleEnum.regular
      )
    {
      foreach (AppUser user in authUsers)
      {
        _appUserHelper.AssignRoles(user, new AppRoleEnum[] { basicRole });
      }
    }

    public bool PersistModels(WorkshopManagerContext dbAccess)
    {
      if (Models.Length == 0)
        LoadJSONModels();

      PersistSystemUsers(dbAccess, Models);

      var usernames = Models
        .Select(m => m.UserName)
        .ToArray();
      var authUsers = dbAccess.Users
        .Where(u => usernames
        .Contains(u.Username))
        .ToArray();

      AssignBasicRoles(authUsers, dbAccess);

      foreach (Mechanician m in Models)
      {
        var authUser = authUsers.FirstOrDefault(u => u.Username.Equals(m.UserName));
        if (authUser != null)
        {
          m.AppUserId = authUser.Id;
          m.AppUser = authUser;
        }
      }

      Models = Models.Where(m => m.AppUser != null).ToArray();
      int res = -1;
      if (Models.Length > 0)
      {
        dbAccess.Mechanicians.AddRange(Models);
        dbAccess.BulkInsert(Models);
        res = Models.Length;
      }
      if (res != -1)
      {
        Models = dbAccess.Workers.OfType<Mechanician>().ToList().ToArray();
        return true;
      }
      else
      {
        return false;
      }
    }

    public async Task<bool> InsertModelsAsync()
    {
      throw new NotImplementedException();
    }

    public void InsertModelsAndRelatedData()
    {
      using (var dbAccess = new WorkshopManagerContext())
      {
        Models = dbAccess.Workers.OfType<Mechanician>().ToList().ToArray();
        if (Models.Length == 0)
          PersistModels(dbAccess);
      }
    }

    /// <summary>
    /// Zwraca przynajmniej jednego losowo wybranego mechanika.
    /// </summary>
    /// <param name="rounds">Liczba losowań indeksów - im większa tym większe prawdopodbieństwo że do zlecenia przydzielona zostanie większa ilość mechaników.</param>
    /// <param name="src">Zbiór na który będą wskazywać wylosowane indeksy (kolekcja źródłowa losowanych obiektów).</param>
    /// <returns></returns>
    private Mechanician[] GetAtLeastOneRandomFrom(Mechanician[] src = null, int rounds = 10, int? limit = null)
    {
      if (src == null)
      {
        src = Models;
      }
      if (limit == null)
      {
        limit = _maxMechanciansPerOrder;
      }
      List<Mechanician> selectedMechs = new List<Mechanician>();
      var _rand = new Random();
      var indexes = new List<int>();
      for (int i = 0; i < rounds; i++)
      {
        int index = _rand.Next(0, src.Length);
        if (!indexes.Contains(index))
          indexes.Add(index);

        if (indexes.Count >= limit.Value)
          break;
      }
      for (int i = 0; i < src.Length; i++)
      {
        if (indexes.Contains(i))
          selectedMechs.Add(src[i]);
      }
      return selectedMechs.ToArray();
    }

    public bool MatchRandomlyWithExistingOrders()
    {
      InsertModelsAndRelatedData();
      var bindings = new List<OrderToWorker>();
      try
      {
        using (var dbAccess = new WorkshopManagerContext())
        {
          var orders = dbAccess.Orders.ToArray();
          foreach (Order order in orders)
          {
            var mechanicians = GetAtLeastOneRandomFrom();
            var supervisor = GetAtLeastOneRandomFrom(mechanicians, 1, 1).SingleOrDefault();
            if (supervisor != null)
            {
              order.SupervisorId = supervisor.Id;
            }
            foreach (Mechanician mechanician in mechanicians)
            {
              bindings.Add(new OrderToWorker() { OrderId = order.Id, WorkerId = mechanician.Id });
            }
          }
          dbAccess.BulkInsert<OrderToWorker>(bindings);
          dbAccess.SaveChanges();
        }
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return false;
      }
    }

    public void LoadJSONModels()
    {
      Models = JsonReader.GetModels().ToArray();
    }
  }
}
