﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkshopManagerNET.Model
{
  [Table("TimeLog")]
  public class TimeLog
  {
    [Key]
    public long Id { get; set; }

    [Column(TypeName = "datetime2(7)")]
    public DateTime LogTime { get; set; }

    [Column(TypeName = "decimal(3,1)")]
    public decimal Hours { get; set; }

    [ForeignKey("Worker")]
    public long WorkerId { get; set; }

    [ForeignKey("Order")]
    public long OrderId { get; set; }

    public Worker Worker { get; set; }
    public Order Order { get; set; }
  }
}
