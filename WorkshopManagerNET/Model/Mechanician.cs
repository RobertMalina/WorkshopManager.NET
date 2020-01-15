using System;
using System.Collections.Generic;
using System.Text;

namespace WorkshopManagerNET.Model
{
  public enum SpecializationEnum {
    New,
    Any,
    Suspension, 
    PetrolEngine, 
    DieselEngine,
    Gearboxes,
    Turbochargers,
  }
  public class Mechanician: Worker
  {
    public SpecializationEnum Specialization { get; set; }

    public int? RepairmentsCount { get; set; }
  }
}
