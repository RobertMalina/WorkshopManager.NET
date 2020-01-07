using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkshopManagerNET.Model
{
  public enum OrderStatusEnum { Unknown, Registered, InProgress, Finished }

  [Table("Order")]
  class Order
  {
    [Key]
    public long Id { get; set; }

    [Required]
    [ForeignKey("Client")]
    public long ClientId { get; set; }

    [ForeignKey("Supervisor")]
    public long SupervisorId { get; set; }
    public Worker Supervisor { get; set; }

    [Required]
    [MaxLength(128)]
    [Column(TypeName = "nvarchar(128)")]
    public string Title { get; set; }

    [Required]
    [MaxLength(128)]
    [Column(TypeName = "nvarchar(128)")]
    public string VehicleDescription { get; set; }

    [Column(TypeName = "nvarchar(MAX)")]
    public string Description { get; set; }

    [Required]
    [Column(TypeName = "datetime2(7)")]
    public DateTime DateRegister { get; set; }

    [Column(TypeName = "datetime2(7)")]
    public DateTime? DateStart { get; set; }
    
    [Column(TypeName = "datetime2(7)")]
    public DateTime? DateEnd { get; set; }

    [Column(TypeName = "decimal(9,2)")]
    public decimal Cost { get; set; }

    [Column(TypeName = "decimal(3,1)")]
    public decimal EstimatedTimeInHours { get; set; }
    public Client Client { get; set; }
    public ICollection<OrderToWorker> WorkerOrders { get; set; }
    public ICollection<Part> Parts { get; set; }
    public OrderStatusEnum Status { get; set; }
    public bool Archived { get; set; }
  }
}
