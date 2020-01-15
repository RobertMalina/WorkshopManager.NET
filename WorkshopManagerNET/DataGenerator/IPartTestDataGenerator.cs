using System;
using System.Collections.Generic;
using System.Text;

namespace WorkshopManager.net.DataGenerator
{
  public interface IPartTestDataGenerator
  {
    string GenerateDescription();
    string GenerateCode();
    decimal GeneratePrice();
  }
}
