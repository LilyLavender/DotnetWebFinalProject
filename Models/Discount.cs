using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
public class Discount
{
  public int DiscountId { get; set; }
  public int Code { get; set; }
  public DateTime? StartTime { get; set; }
  public DateTime? EndTime { get; set; }
  public int ProductId { get; set; }
  [Column(TypeName = "decimal(4,4)")]
  public decimal DiscountPercent { get; set; }
  [Required]
  public string Title { get; set; }
  public string Description { get; set; }

  public Product Product { get; set; }
}
