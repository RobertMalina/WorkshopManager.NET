using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkshopManagerNET.Model
{
  [Table("Part")]
  class Part
  {
    [Key]
    public long Id { get; set; }
    public long ParentPartSetId { get; set; }
    public virtual Part ParentalPartSet { get; set; }
    public virtual ICollection<Part> SubParts { get; set; }

    [Required]
    [MaxLength(64)]
    [Column(TypeName = "nvarchar(64)")]
    public string Code { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(MAX)")]
    public string Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(9,2)")]
    public decimal Price { get; set; }

    [Required]
    [ForeignKey("Order")]
    public long OrderId { get; set; }
    
    public Order Order { get; set; }
  }
}
