using System.ComponentModel.DataAnnotations;

public class PurchaseEntity
{
    [Key]
    public int purchaseId { get; set; }
    public required ProductEntity item { get; set; }
    public double unit { get; set; }
}
