using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkshopManagerNET.Model
{
  [Table("Order")]
  class Order
  {
    [Key]
    public long Id { get; set; }

    [Required]
    [ForeignKey("Client")]
    public long ClientId { get; set; }

    [Required]
    [MaxLength(128)]
    [Column(TypeName = "nvarchar(128)")]
    public string Title { get; set; }

    [Required]
    [MaxLength(128)]
    [Column(TypeName = "nvarchar(128)")]
    public string VehicleModel { get; set; }

    [Column(TypeName = "nvarchar(MAX)")]
    public string Description { get; set; }

    [Required]
    [Column(TypeName = "datetime2(7)")]
    public DateTime DateStart { get; set; }

    [Column(TypeName = "datetime2(7)")]
    public DateTime? DateEnd { get; set; }

    [Column(TypeName = "decimal(9,2)")]
    public decimal Price { get; set; }
    public byte PartsCount { get; set; }
    public Client Client { get; set; }
    public ICollection<OrderToWorker> WorkerOrders { get; set; }
    public ICollection<Part> Parts { get; set; }
  }
}
