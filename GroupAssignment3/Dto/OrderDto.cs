namespace GroupAssignment3.Dto
{
    public class OrderEntityDto
    {
        public string orderId { get; set; }
        public required DateTime orderTime { get; set; }
        public required string username { get; set; }
        public required string transactionID { get; set; }
        public required string productId { get; set; }
        public required double pricing { get; set; }
        public required double shippingCost { get; set; }
        public required double quantity { get; set; }
        public required double totalPrice { get; set; }
        public required double totalTaxes { get; set; }
    }
    public class OrderDetail
    {
        public OrderEntityDto order { get; set; }
        public ProductEntity product { get; set; }

    }

    public class OrderDto
    {
        public OrderDto(string tansactionID)
        {
            TansactionID = tansactionID;
            TotalQty = 0;
            TotalPrice = 0;
            TotalShipping = 0;
            TotalTaxes = 0;
            Detail = new List<OrderDetail>();
        }

        public string TansactionID { get; set; }
        public double TotalQty { get; set; }
        public double TotalPrice { get; set; }
        public double TotalShipping { get; set; }
        public double TotalTaxes { get; set; }
        public DateTime Date { get; set; }
        public List<OrderDetail> Detail { get; set; }

    }
}
