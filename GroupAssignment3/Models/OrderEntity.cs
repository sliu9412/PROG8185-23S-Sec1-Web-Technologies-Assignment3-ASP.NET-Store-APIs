using System.ComponentModel.DataAnnotations;

public class OrderEntity
{
    [Key]
    public string orderId { get; set; }
    public required DateTime orderTime { get; set; }
    public required UserEntity user { get; set; }
    public required string transactionID { get; set; }
    public required string productId { get; set; }
    public required double pricing { get; set; }
    public required double shippingCost { get; set; }
    public required double quantity { get; set; }
    public required double totalPrice { get; set; }
    public required double totalTaxes { get; set; }

}