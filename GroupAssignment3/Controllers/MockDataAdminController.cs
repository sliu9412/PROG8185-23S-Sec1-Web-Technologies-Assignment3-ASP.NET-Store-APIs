using GroupAssignment3.Dto;
using GroupAssignment3.Identity;
using GroupAssignment3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GroupAssignment3.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MockDataAdminController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContext;
        private readonly UserService userService;
        private readonly ProductService productService;
        private readonly CartService cartService;
        private readonly OrderService orderService;
        private readonly ProductCommentService productCommentService;

        public MockDataAdminController(
            IHttpContextAccessor httpContext,
            UserService userService,
            ProductService productService,
            CartService cartService,
            OrderService orderService,
            ProductCommentService productCommentService
        )
        {
            this.httpContext = httpContext;
            this.userService = userService;
            this.productService = productService;
            this.cartService = cartService;
            this.orderService = orderService;
            this.productCommentService = productCommentService;
        }

        [HttpGet("v1/admin/mockdata")]
        [Authorize(Policy = IdentityData.ScopeAdminPolicyName)]
        public IActionResult initUsers()
        {
            ProductEntity prod1 = this.productService.CreateProduct(new Dto.CreateProductDto
            {
                description = "This is a description of this product 1",
                image = "images/NikonDSLR.jpg",
                pricing = 7.0,
                shippingCost = 2.0
            });
            ProductEntity prod2 = this.productService.CreateProduct(new Dto.CreateProductDto
            {
                description = "This is a description of this product 2",
                image = "images/CannonDSLR.jpg",
                pricing = 8.0,
                shippingCost = 2.0
            });
            ProductEntity prod3 = this.productService.CreateProduct(new Dto.CreateProductDto
            {
                description = "This is a description of this product 3",
                image = "images/CannonDSLR.jpg",
                pricing = 8.0,
                shippingCost = 2.0
            });
            ProductEntity prod4 = this.productService.CreateProduct(new Dto.CreateProductDto
            {
                description = "This is a description of this product 4",
                image = "images/NikonDSLR.jpg",
                pricing = 9.0,
                shippingCost = 2.0
            });

            UserEntity user1 = this.userService.CreateUser(new Dto.UserCreateDto
            {
                email = "eugenio@mail.com",
                password = "password",
                username = "eugenio",
                shippingAddress = "some street 1"
            });

            this.cartService.AddItemToTheCart(user1.userId, prod1.productId, 2);
            this.cartService.AddItemToTheCart(user1.userId, prod2.productId, 1);
            this.cartService.AddItemToTheCart(user1.userId, prod3.productId, 4);
            this.cartService.AddItemToTheCart(user1.userId, prod4.productId, 3);

            this.orderService.purcharseUserCart(user1.userId);

            this.cartService.AddItemToTheCart(user1.userId, prod2.productId, 1);

            ProductCommentDto comment = new ProductCommentDto();
            comment.ProductId = prod1.productId;
            comment.Comment = "My first comment for " + prod1.description;
            comment.Rating = 4;
            comment.images = new List<Dto.CommentImage>();
            comment.images.Add(new Dto.CommentImage
            {
                ImagePath = "images/prod1image1.jpg",
            });
            comment.images.Add(new Dto.CommentImage
            {
                ImagePath = "images/prod1image2.jpg",
            });
            this.productCommentService.MakeComment(user1.userId, comment);

            return Ok("Database has been initialized with mock data");
        }

    }
}
