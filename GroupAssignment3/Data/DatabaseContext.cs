using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GroupAssignment3.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

        public required DbSet<ProductEntity> Product { get; set; }
        public required DbSet<UserEntity> User { get; set; }
        public required DbSet<CommentsEntity> Comments { get; set; }
        public required DbSet<CommentImage> CommentImage { get; set; }
        public required DbSet<CartEntity> Cart { get; set; }
        public required DbSet<OrderEntity> Order { get; set; }
        public required DbSet<PurchaseEntity> Purchase { get; set; }
    }
}