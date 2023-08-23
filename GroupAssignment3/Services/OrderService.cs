using GroupAssignment3.Data;
using GroupAssignment3.Dto;
using GroupAssignment3.Exception;
using System.Collections.Generic;

namespace GroupAssignment3.Services
{
    public class OrderService
    {

        private readonly CartService cartService;
        private readonly DatabaseContext context;
        private readonly UserService userService;
        private readonly ProductService productService;

        public OrderService(CartService cartService, DatabaseContext context, UserService userService, ProductService productService)
        {
            this.cartService = cartService;
            this.context = context;
            this.userService = userService;
            this.productService = productService;
        }

        public List<OrderEntity> purcharseUserCart(string userId)
        {
            UserCartDto userCart = this.cartService.GetEntireCart(userId);
            string transactionID = Guid.NewGuid().ToString();
            UserEntity user = this.userService.GetUserById(userId);
            List<OrderEntity> orderToPurchase = new List<OrderEntity>();
            foreach (CartDto cartItem in userCart.CartItems)
            {
                OrderEntity orderItem = new OrderEntity()
                {
                    orderId = Guid.NewGuid().ToString(),
                    orderTime = DateTime.Now,
                    pricing = cartItem.TotalPrice,
                    productId = cartItem.Product.productId,
                    quantity = cartItem.TotalQty,
                    shippingCost = cartItem.TotalShippingCost,
                    totalPrice = cartItem.TotalPrice,
                    totalTaxes = Math.Round(cartItem.TotalTaxes, 2, MidpointRounding.ToEven),
                    transactionID = transactionID,
                    user = user
                };
                orderToPurchase.Add(orderItem);
            }
            context.AddRange(orderToPurchase);
            context.SaveChanges();
            this.cartService.DeleteCartByUserId(user);
            return orderToPurchase;
        }

        public List<OrderDto> getOrdersHistoryByUserId(string userId)
        {
            UserEntity user = this.userService.GetUserById(userId);
            // Get the user order history
            List<OrderEntity> orderHist = this.getOrderByUserId(user);
            if (orderHist == null || orderHist.Count == 0)
            {
                throw new HttpException(404, "User does not have orders");
            }
            // Group history by transaction ID
            Dictionary<string, List<OrderEntity>> groupByTrxId = new Dictionary<string, List<OrderEntity>>();
            foreach (OrderEntity order in orderHist)
            {
                List<OrderEntity> grouped = groupByTrxId.GetValueOrDefault(order.transactionID);
                if (grouped == null)
                {
                    grouped = new List<OrderEntity>();
                    groupByTrxId.Add(order.transactionID, grouped);
                }
                grouped.Add(order);
            }
            // Create response calculating values by transaction ID and defining their product details
            List<OrderDto> response = new List<OrderDto>();
            foreach (KeyValuePair<string, List<OrderEntity>> entry in groupByTrxId)
            {
                OrderDto dtoOrderHist = new OrderDto(entry.Key);
                foreach (OrderEntity orderDB in entry.Value)
                {
                    dtoOrderHist.TotalTaxes += Math.Round(orderDB.totalTaxes, 2, MidpointRounding.ToEven);
                    dtoOrderHist.TotalQty += orderDB.quantity;
                    dtoOrderHist.TotalPrice += orderDB.totalPrice;
                    dtoOrderHist.TotalShipping += orderDB.shippingCost;
                    dtoOrderHist.Date = orderDB.orderTime;
                    ProductEntity product = this.productService.GetProductById(orderDB.productId);
                    dtoOrderHist.Detail.Add(new OrderDetail
                    {
                        order = new OrderEntityDto
                        {
                            orderId = orderDB.orderId,
                            orderTime = orderDB.orderTime,
                            pricing = orderDB.pricing,
                            productId = orderDB.productId,
                            quantity = orderDB.quantity,
                            shippingCost = orderDB.shippingCost,
                            totalPrice = orderDB.totalPrice,
                            totalTaxes = orderDB.totalTaxes,
                            transactionID = orderDB.transactionID,
                            username = orderDB.user.username
                        },
                        product = product
                    });
                }
                response.Add(dtoOrderHist);
            }
            return response;
        }

        public bool IsUserHasOrderForProduct(string userId, string productId)
        {
            OrderEntity order = this.context.Order
                .Where(o => o.user.userId == userId && o.productId == productId)
                .FirstOrDefault();
            return order != null;
        }

        private List<OrderEntity> getOrderByUserId(UserEntity user)
        {
            return this.context.Order
                .Where(o => o.user.userId == user.userId)
                .OrderByDescending(o => o.orderTime)
                .ToList();
        }

    }
}
