namespace GroupAssignment3.Dto
{ 

    public class CartDto
    {
        public double TotalPrice { get; set; }
        public double TotalQty { get; set; }
        public double TotalTaxes { get; set; }
        public double TotalShippingCost { get; set; }
        public ProductEntity Product { get; set; }

    }

    public class UserCartDto
    {
        public UserCartDto()
        {
            TotalPrice = 0;
            TotalShippingCost = 0;
            TotalQty = 0;
            TotalTaxes = 0;
            CartItems = new List<CartDto>();
        }

        public string CartID { get; set; }

        public double TotalPrice { get; set; }
        public double TotalShippingCost { get; set; }
        public double TotalQty { get; set; }
        public double TotalTaxes { get; set; }

        public List<CartDto> CartItems { get; set; }
    }

    public class AddCartItem
    {
        public string ProductId { get; set; }

        public double Qty { get; set; }
    }

}
