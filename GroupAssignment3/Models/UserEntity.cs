using System.ComponentModel.DataAnnotations;

public class UserEntity
{
    [Key]
    public required string userId { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public string username { get; set; }
    public string shippingAddress { get; set; }
    public bool isActive { get; set; }
}