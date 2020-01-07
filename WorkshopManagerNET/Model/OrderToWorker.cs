using System.ComponentModel.DataAnnotations.Schema;

namespace WorkshopManagerNET.Model
{
  [Table("OrderToWorker")]
  class OrderToWorker
  {
    public Order Order { get; set; }
    public long OrderId { get; set; }
    public Worker Worker { get; set; }
    public long WorkerId { get; set; }
  }
}
