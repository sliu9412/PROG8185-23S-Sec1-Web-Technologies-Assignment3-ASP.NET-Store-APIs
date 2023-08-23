using GroupAssignment3.Dto;
using System.Xml.Linq;

namespace GroupAssignment3.Helper
{
    public class RestFormat
    {

        public static Object UserEntity(UserEntity u) => new
        {
            u.userId,
            u.username,
            u.email,
            u.shippingAddress,
        };

        public static Object UserEntity(UserEntity u, string jwtToken) => new
        {
            u.username,
            u.email,
            Token = jwtToken
        };

        public static Object OrderEntities(List<OrderEntity> orders)
        {
            if (orders == null)
            {
                return new List<Object>();
            }
            return orders.Select(c => new
            {
                c.orderId,
                c.orderTime,
                c.transactionID,
                c.productId,
                c.quantity,
                c.pricing,
                c.shippingCost,
                c.totalPrice,
                c.totalTaxes,
                c.user.username
            }).ToList();
        }

        public static Object CommentEntity(List<CommentsEntity> comments)
        {
            if (comments == null)
            {
                return new List<Object>();
            }
            return comments.Select(c => new
            {
                c.commentId,
                c.rating,
                c.text,
                c.commentImages,
                c.user.username
            }).ToList();
        }

    }
}
