using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkshopManagerNET.Model.Auth
{
  [Table("AppUser")]
  public class AppUser
  {
    [Key]
    public long Id { get; set; }
    [Required]
    [MaxLength(64)]
    [Column(TypeName = "nvarchar(128)")]
    public string Username { get; set; }
    [Required]
    [MaxLength(64)]
    [Column(TypeName = "nvarchar(64)")]
    public string PasswordHash { get; set; }
    public ICollection<AppUserToAppRole> Roles { get; set; }

    //[ForeignKey("Worker")]
    //public long WorkerId { get; set; }
    public Worker Worker { get; set; }
  }
}
