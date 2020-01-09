using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkshopManagerNET.Model
{
  [Table("Trainee")]
  public class Trainee
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
    [ForeignKey("Supervisor")]
    public long SupervisorId { get; set; }  
    public Worker Supervisor { get; set; }
  }
}
