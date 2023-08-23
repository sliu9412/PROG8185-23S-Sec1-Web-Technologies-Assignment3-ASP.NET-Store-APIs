using System.ComponentModel.DataAnnotations;

public class CartEntity
{
    [Key]
    public string CartId { get; set; }
    public required UserEntity user { get; set; }
    public ICollection<PurchaseEntity> purchaseItems { get; set; }
}