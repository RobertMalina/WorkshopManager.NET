using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkshopManagerNET.Model
{
  [Table("Client")]
  public partial class Client
  {
    [Key]
    public long Id { get; set; }

    [Required]
    [MaxLength(64)]
    [Column(TypeName = "nvarchar(64)")]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(64)]
    [Column(TypeName = "nvarchar(64)")]
    public string LastName { get; set; }

    [Required]
    [StringLength(10, MinimumLength = 8)]
    [Column(TypeName = "char(10)")]
    public string PhoneNumber { get; set; }
    public ICollection<Order> Orders { get; set; }
  }
}
