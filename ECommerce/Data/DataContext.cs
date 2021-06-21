using ECommerce.Data.Models;
using ECommerce.Helpers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Data
{
    public class DataContext:IdentityDbContext<ApiUser>
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.Entity<CartItem>().HasKey(ci => new { ci.UserId, ci.ProductId });
            builder.Entity<OrderItem>().HasKey(oi => new { oi.OrderId, oi.ProductId });
            builder.Entity<Order>().Property(o => o.Confirmed).HasDefaultValue(false);
            builder.Entity<Order>().Property(o => o.Sent).HasDefaultValue(false);
            builder.Entity<Order>().Property(o => o.CreatedAt).HasDefaultValueSql("getdate()");
            builder.Entity<Product>().Property(p => p.New).HasDefaultValue(false);
            builder.Entity<Product>().Property(p => p.Featured).HasDefaultValue(false);
            builder.Entity<Product>().Property(p => p.CreatedAt).HasDefaultValueSql("getdate()");






            builder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p=>p.Category)
                .HasForeignKey(p=>p.CategoryId).IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
           
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> Cart { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
    }
}
