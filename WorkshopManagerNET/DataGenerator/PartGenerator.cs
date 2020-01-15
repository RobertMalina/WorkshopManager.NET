using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using WorkshopManagerNET.Model;

namespace WorkshopManager.net.DataGenerator
{
  class PartTestDataHelper : IPartTestDataGenerator
  {
    private Random _rand;
    public PartTestDataHelper()
    {
      _rand = new Random();
    }
    public string GenerateCode()
    {
      var g = Guid.NewGuid();
      var partUniqueCode = Convert.ToBase64String(g.ToByteArray());
      partUniqueCode = partUniqueCode.Replace("=", "");
      partUniqueCode = partUniqueCode.Replace("+", "");
      return $"Part#{partUniqueCode}";
    }

    public string GenerateDescription()
    {
      return "Part description";
    }

    public decimal GeneratePrice()
    {
      bool isBig = _rand.Next(1, 11) <= 2; //20% odds for "big" price
      int minTenthCount = isBig ? 30 : 1;
      int maxTenthCount = isBig ? 150 : 20;

      var price = _rand.Next(minTenthCount, maxTenthCount) * 10;
      double modifier = _rand.Next(1, 3) % 2 == 0 ? 0.01 : 0.51;
      decimal result = Convert.ToDecimal(price - modifier);

      return result;
    }
  }
  class PartGenerator
  {
    private const int _maxSubPartsCount = 5;
    private IPartTestDataGenerator _partDataGenerator;

    public PartGenerator(IPartTestDataGenerator partDataGenerator)
    {
      _partDataGenerator = partDataGenerator;
    }
    public bool GenerateFor(Order[] orders)
    {
      try
      {
        List<Part> parts = new List<Part>();
        bool createSubParts;
        int subPartsCount;
        var _rand = new Random();

        foreach (Order order in orders)
        {
          var part = CreatePart(order.Id);
          createSubParts = _rand.Next(1, 3) % 2 == 0;

          if (createSubParts)
          {
            subPartsCount = _rand.Next(1, _maxSubPartsCount + 1);
            var subParts = CreateSubPartsOf(order.Id, part, subPartsCount);
            part.SubParts = subParts;
            parts.AddRange(subParts);
            part.SubParts = subParts;
          }
          parts.Add(part);
        }

        using (var dbAccess = new WorkshopManagerContext())
        {
          dbAccess.BulkInsert<Part>(parts);
        }
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return false;
      }
    }

    private Part CreatePart(long orderId, Part parentalPart = null, Part[] subParts = null)
    {
      return new Part()
      {
        OrderId = orderId,
        ParentalPartSet = parentalPart,
        Code = _partDataGenerator.GenerateCode(),
        Description = _partDataGenerator.GenerateDescription(),
        SubParts = subParts,
        Price = _partDataGenerator.GeneratePrice()
      };
    }

    private Part[] CreateSubPartsOf(long orderId, Part parentalPart, int partsCount)
    {
      Part[] subParts = new Part[partsCount];
      for (int i = 0; i < partsCount; i++)
      {
        subParts[i] = CreatePart(orderId, parentalPart);
      }
      return subParts;
    }
  }
}
