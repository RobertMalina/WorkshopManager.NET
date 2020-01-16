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

    private const int _maxMechanciansPerOrder = 3;

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
          LoadJSONModels();
        }
        return _models;
      }
      set => _models = value;
    }

    public bool PersistModels(WorkshopManagerContext dbAccess)
    {
      if (Models.Length == 0)
        LoadJSONModels();

      var httpModule = new HttpModule();

      List<AppUser> authUsers = new List<AppUser>();

      foreach (Mechanician m in Models)
      {
        AppUser authUser = null;
        var body = new Dictionary<string, string>();
        body.Add("Username", m.UserName);
        body.Add("Password", "zaq12wsx");
        authUser = httpModule.Post<AppUser>(bcryptServiceAddr, body).Result;
        if (authUser != null)
        {
          authUsers.Add(authUser);
        }
      }
      dbAccess.BulkInsert<AppUser>(authUsers);
      dbAccess.SaveChanges();

      var usernames = Models.Select(m => m.UserName).ToArray();
      authUsers = dbAccess.Users.Where(u => usernames.Contains(u.Username)).ToList();

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
        try
        {
          dbAccess.Mechanicians.AddRange(Models);
          dbAccess.BulkInsert(Models);
          res = Models.Length;
        }
        catch {}      
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

    public void LoadDbModels()
    {
      try
      {
        using (var dbAccess = new WorkshopManagerContext())
        {
          Models = dbAccess.Workers.OfType<Mechanician>().ToList().ToArray();
          if (Models.Length == 0)
            PersistModels(dbAccess);
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
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

    public bool MatchRandomlyWith(Order[] orders)
    {
      LoadDbModels();
      var bindings = new List<OrderToWorker>();

      foreach (Order order in orders)
      {
        var mechanicians = GetAtLeastOneRandomFrom();
        var supervisor = GetAtLeastOneRandomFrom(mechanicians, 1, 1).SingleOrDefault();
        if (supervisor != null)
        {
          order.SupervisorId = supervisor.Id;
          order.Supervisor = supervisor;
        }
        foreach (Mechanician mechanician in mechanicians)
        {
          bindings.Add(new OrderToWorker() { OrderId = order.Id, WorkerId = mechanician.Id });
        }
      }
      try
      {
        InsertOrderToWorkerBindings(bindings.ToArray());
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return false;
      }
    }

    private void InsertOrderToWorkerBindings(OrderToWorker[] bindings)
    {
      using (var dbAccess = new WorkshopManagerContext())
      {
        dbAccess.BulkInsert<OrderToWorker>(bindings);
      }
    }
    public void LoadJSONModels()
    {
      Models = JsonReader.GetModels().ToArray();
    }
  }
}
