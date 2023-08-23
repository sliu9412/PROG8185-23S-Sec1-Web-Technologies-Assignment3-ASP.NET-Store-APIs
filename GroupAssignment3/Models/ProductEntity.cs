using System.ComponentModel.DataAnnotations;

public class ProductEntity
{
    [Key]
    public string productId { get; set; }
    public string description { get; set; }
    public string image { get; set; }
    public double pricing { get; set; }
    public double shippingCost { get; set; }
}