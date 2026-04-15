using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Ecommerce.Domain.Entities.IdentityModule;
using Ecommerce.Domain.Entities.CartModule;
using Ecommerce.Domain.Entities.OrderModule;

namespace Ecommerce.Infrastructure
{
    public class EcommerceDbContext : IdentityDbContext<ApplicationUser>
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<FavoriteList> FavoriteLists { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<OrderAddress> OrderAddresses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>().OwnsOne(o => o.address, a =>
            {
                a.WithOwner();
            });

            modelBuilder.Entity<OrderItem>().OwnsOne(oi => oi.Product, p =>
            {
                p.WithOwner();
            });

            modelBuilder.Entity<Order>()
                .Property(o => o.OrderStatus)
                .HasConversion(
                    s => s.ToString(),
                    s => (OrderStatus)Enum.Parse(typeof(OrderStatus), s)
                );

            modelBuilder.Entity<Order>().Property(o => o.SubTotal).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<OrderItem>().Property(oi => oi.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<DeliveryMethod>().Property(dm => dm.Price).HasColumnType("decimal(18,2)");
        }
    }
}
