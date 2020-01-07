using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkshopManagerNET.Model
{
  class Department
  {
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(128)]
    [Column(TypeName = "nvarchar(128)")]
    public string Name { get; set; }
    public string Description { get; set; }
  }
}
