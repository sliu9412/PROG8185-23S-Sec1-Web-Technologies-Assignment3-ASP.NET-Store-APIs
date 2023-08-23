using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroupAssignment3.Data;
using GroupAssignment3.Dto;
using GroupAssignment3.Helper;
using GroupAssignment3.Identity;
using GroupAssignment3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace GroupAssignment3.Controllers
{
    [ApiController]
    [Authorize(Policy = IdentityData.ScopeUserPolicyName)]
    [Route("[controller]")]
    public class CartApiController : ControllerBase
    {

        private readonly CartService cartService;
        private readonly OrderService orderService;
        private readonly IHttpContextAccessor httpContext;

        public CartApiController(CartService cartService, IHttpContextAccessor httpContext, OrderService orderService)
        {
            this.cartService = cartService;
            this.httpContext = httpContext;
            this.orderService = orderService;

        }

        [HttpGet("v1/cart")]
        public IActionResult GetUserCart()
        {
            string userId = JwtHelper.GetSessionFromToken(this.httpContext).userId;
            return Ok(this.cartService.GetEntireCart(userId));
        }

        [HttpPost("v1/cart")]
        public IActionResult PurchaseCart([FromBody] AddCartItem cartItem)
        {
            string userId = JwtHelper.GetSessionFromToken(this.httpContext).userId;
            return Ok(this.cartService.AddItemToTheCart(userId, cartItem.ProductId, cartItem.Qty));
        }

        [HttpPost("v1/cart/purchase")]
        public IActionResult PurchaseCart()
        {
            string userId = JwtHelper.GetSessionFromToken(this.httpContext).userId;
            return Ok(RestFormat.OrderEntities(this.orderService.purcharseUserCart(userId)));
        }

    }
}