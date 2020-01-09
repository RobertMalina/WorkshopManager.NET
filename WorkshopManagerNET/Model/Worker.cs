using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkshopManagerNET.Model.Auth;

namespace WorkshopManagerNET.Model
{
  [Table("Worker")]
  public partial class Worker
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
    public short? Bid { get; set; }
  
    public ICollection<OrderToWorker> WorkerOrders { get; set; }
    public Trainee Trainee { get; set; }
    public ICollection<Order> SupervisedOrders { get; set; }
    public ICollection<TimeLog> TimeLogs { get; set; }

    [ForeignKey("AppUser")]
    public long AppUserId { get; set; }
    public AppUser AppUser { get; set; }
  }
}
