using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkshopManagerNET.Model
{
  [Table("AppRole")]
  public class AppRole
  {
    [Key]
    public int Id { get; set; }

    [MaxLength(64)]
    [Column(TypeName = "nvarchar(64)")]
    public string Name { get; set; }

    public ICollection<AppUserToAppRole> Users { get; set; }
  }
}
