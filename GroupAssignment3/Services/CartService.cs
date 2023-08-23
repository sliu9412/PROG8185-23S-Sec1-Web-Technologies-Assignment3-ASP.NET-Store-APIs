using GroupAssignment3.Data;
using GroupAssignment3.Dto;
using GroupAssignment3.Exception;
using Microsoft.EntityFrameworkCore;

namespace GroupAssignment3.Services
{
    public class CartService
    {

        private readonly DatabaseContext context;
        private readonly UserService userService;
        private readonly ProductService productService;
        private double taxesConstant;

        public CartService(DatabaseContext context, ProductService productService, UserService userService)
        {
            this.context = context;
            this.userService = userService;
            this.productService = productService;
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            this.taxesConstant = Double.Parse(config["Constants:Taxes"]!);
        }

        public UserCartDto AddItemToTheCart(string userId, string productId, double qty)
        {
            UserEntity user = this.userService.GetUserById(userId);
            ProductEntity product = this.productService.GetProductById(productId);
            CartEntity userCart = this.GetCartByUserId(user, true);
            if (userCart.purchaseItems == null)
            {
                userCart.purchaseItems = new List<PurchaseEntity>();
            }
            PurchaseEntity itemExists = userCart.purchaseItems
                .Where(purchaseItem => purchaseItem.item.productId == productId)
                .SingleOrDefault();
            if (itemExists != null)
            {
                if (qty == 0)
                {
                    this.context.Purchase.Remove(itemExists);
                    bool deletedItems = userCart.purchaseItems.Remove(itemExists);                }
                else
                {
                    itemExists.unit = qty;
                }
            } 
            else
            {
                if (qty == 0)
                {
                    throw new HttpException(400, "Quantity must be greather than 0");
                }
                userCart.purchaseItems.Add(new PurchaseEntity()
                {
                    item = product,
                    unit = qty
                });
            }
            context.SaveChanges();
            return GetEntireCart(userId);
        }

        public UserCartDto GetEntireCart(string userId)
        {
            UserEntity user = this.userService.GetUserById(userId);
            CartEntity userCart = this.GetCartByUserId(user, false);
            if (userCart == null || userCart.purchaseItems == null || userCart.purchaseItems.Count == 0)
            {
                throw new HttpException(404, "User ID '" + userId + "' does not have cart items");
            }
            UserCartDto userCartDto = new UserCartDto();
            userCartDto.CartID = userCart.CartId;
            foreach (PurchaseEntity dbCartItem in userCart.purchaseItems)
            {
                CartDto cartItem = new CartDto();
                cartItem.Product = dbCartItem.item;
                cartItem.TotalPrice = (dbCartItem.unit * cartItem.Product.pricing);
                cartItem.TotalQty = dbCartItem.unit;
                cartItem.TotalShippingCost = cartItem.Product.shippingCost;
                cartItem.TotalTaxes = Math.Round((this.taxesConstant * cartItem.TotalPrice), 2, MidpointRounding.ToEven);
                userCartDto.CartItems.Add(cartItem);
                userCartDto.TotalQty += cartItem.TotalQty;
                userCartDto.TotalPrice += cartItem.TotalPrice;
                userCartDto.TotalShippingCost += cartItem.TotalShippingCost;
                userCartDto.TotalTaxes += Math.Round(cartItem.TotalTaxes, 2, MidpointRounding.ToEven);
            }
            return userCartDto;
        }

        public void DeleteCartByUserId(UserEntity user)
        {
            CartEntity cart = GetCartByUserId(user, false);
            if (cart.purchaseItems != null)
            {
                foreach(var cartItem in  cart.purchaseItems)
                {
                    this.context.Purchase.Remove(cartItem);
                }
            }
            this.context.Remove(cart);
            this.context.SaveChanges();
        }

        private CartEntity GetCartByUserId(UserEntity user, bool createIfNotExists)
        {
            CartEntity cart = context.Cart
                .Include(c => c.user)
                .Include(c => c.purchaseItems)
                .ThenInclude(p => p.item)
                .Where(c => c.user.userId == user.userId)
                .FirstOrDefault();
            if (cart == null)
            {
                if (!createIfNotExists)
                {
                    throw new HttpException(404, "User ID '" + user.userId + "' does not have cart");
                }
                cart = CreateUserCart(user);
            }
            return cart;
        }

        private CartEntity CreateUserCart(UserEntity user)
        {
            CartEntity newCart = new CartEntity()
            {
                user = user,
                CartId = Guid.NewGuid().ToString(),
            };
            context.Add(newCart);
            context.SaveChanges();
            return newCart;
        }

    }
}
