using GroupAssignment3.Data;
using GroupAssignment3.Dto;
using GroupAssignment3.Exception;
using Microsoft.EntityFrameworkCore;

namespace GroupAssignment3.Services
{
    public class ProductCommentService
    {
        private readonly UserService userService;
        private readonly ProductService productService;
        private readonly DatabaseContext context;
        private readonly OrderService orderService;

        public ProductCommentService(
            UserService userService, 
            ProductService productService, 
            DatabaseContext context,
            OrderService orderService
        )
        {
            this.userService = userService;
            this.productService = productService;
            this.context = context;
            this.orderService = orderService;
        }

        public void MakeComment(string userId, ProductCommentDto comment)
        {
            UserEntity user = this.userService.GetUserById(userId);
            ProductEntity product = this.productService.GetProductById(comment.ProductId);
            if (!this.orderService.IsUserHasOrderForProduct(userId, product.productId))
            {
                throw new HttpException(500, "This user does not have orders for product '" + 
                    comment.ProductId + "'. Not allowed to make comments.");
            }
            CommentsEntity newComment = new CommentsEntity
            {
                commentId = Guid.NewGuid().ToString(),
                product = product,
                user = user,
                text = comment.Comment
            };
            newComment.rating = comment.Rating;
            if (comment.images != null)
            {
                foreach(var image in comment.images)
                {
                    newComment.commentImages.Add(new CommentImage
                    {
                        imageId = Guid.NewGuid().ToString(),
                        imagePath = image.ImagePath
                    });
                }
            }
            this.context.Add(newComment);
            this.context.SaveChanges();
        }

        public List<CommentsEntity> GetProductComment(string productId)
        {
            ProductEntity product = this.productService.GetProductById(productId);
            return this.context.Comments
                .Include(c => c.commentImages)
                .Include(c => c.product)
                .Include(c => c.user)
                .Where(c => c.product.productId == product.productId)
                .ToList();
        }

        public void DeleteComment(string userId, string commentId)
        {
            UserEntity user = this.userService.GetUserById(userId);
            CommentsEntity comment = this.GetCommentById(userId, commentId);
            if (comment == null)
            {
                throw new HttpException(401, "Comment '" + commentId + "' is not related to user '" + userId + "'.");
            }
            if (comment.commentImages != null)
            {
                foreach(var image in comment.commentImages)
                {
                    this.context.Remove(image);
                }
            }
            this.context.Remove(comment);
            this.context.SaveChanges();
        }

        private CommentsEntity GetCommentById(string userId, string commentId)
        {
            return this.context.Comments
                .Include(c => c.commentImages)
                .Include(c => c.product)
                .Include(c => c.user)
                .Where(c => c.commentId == commentId && c.user.userId == userId)
                .SingleOrDefault();
        }

    }
}
