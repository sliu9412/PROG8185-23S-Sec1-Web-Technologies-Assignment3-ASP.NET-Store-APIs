using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroupAssignment3.Data;
using GroupAssignment3.Helper;
using GroupAssignment3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace GroupAssignment3.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class OrderApiController : ControllerBase
    {
        private readonly OrderService orderService;
        private readonly IHttpContextAccessor httpContext;

        public OrderApiController(OrderService orderService, IHttpContextAccessor httpContext)
        {
            this.orderService = orderService;
            this.httpContext = httpContext;

        }

        // get order list
        [HttpGet("v1/order")]
        public IActionResult GetOrderList()
        {
            string userId = JwtHelper.GetSessionFromToken(this.httpContext).userId;
            return Ok(this.orderService.getOrdersHistoryByUserId(userId));
        }
    }
}