using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroupAssignment3.Data;
using GroupAssignment3.Dto;
using GroupAssignment3.Identity;
using GroupAssignment3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupAssignment3.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ProductApiController : ControllerBase
    {
        private readonly ProductService productService;
        public ProductApiController(ProductService productService)
        {
            this.productService = productService;
        }

        [HttpPost("v1/product")]
        [Authorize(Policy = IdentityData.ScopeAdminPolicyName)]
        public IActionResult CreateNewProduct([FromBody] CreateProductDto product)
        {
            return Ok(this.productService.CreateProduct(product));
        }

        [HttpGet("v1/product")]
        public IActionResult GetAllProducts()
        {
            return Ok(this.productService.GetAllProducts());
        }

    }
}