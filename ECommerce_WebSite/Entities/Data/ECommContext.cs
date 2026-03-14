using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Data
{
    public class ECommContext : IdentityDbContext<App_User>
    {
        public ECommContext()
        {
            
        }
        public ECommContext(DbContextOptions<ECommContext> options): base(options)
        {

        }
        // Entities
        //public DbSet<App_User> App_Users {  get; set; }   // DbSet for the App_User entity, representing the App_User table in the database
        public DbSet<Address> Addresses { get; set; }   // DbSet for the Address entity, representing the Address table in the database
        public DbSet<Order> Orders { get; set; }   // DbSet for the Order entity, representing the Order table in the database
        public DbSet<Order_Item> Order_Items { get; set; }   // DbSet for the Order_Item entity, representing the Order_Item table in the database
        public DbSet<Product> Products { get; set; }   // DbSet for the Product entity, representing the Product table in the database
        public DbSet<Category> Categories { get; set; }   // DbSet for the Category entity, representing the Category table in the database
        // Fluent API to configure constraints on database
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // MUST call the base method first to configure all Identity tables (like AspNetUsers, AspNetUserLogins, etc.)
            base.OnModelCreating(builder);
            // 2. Override the default table name for the user
            builder.Entity<App_User>().ToTable("App_User");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            // Explicitly set the table names to singular
            builder.Entity<Product>().ToTable("Product");
            builder.Entity<Category>().ToTable("Category");
            builder.Entity<Order>().ToTable("Order");
            builder.Entity<Order_Item>().ToTable("Order_Item");
            builder.Entity<Address>().ToTable("Address");

            builder.Entity<Order>(o =>
            {
                o.HasIndex(p => p.OrderNumber).IsUnique(true);
                // Configure decimal precision (e.g., 10 digits total, 2 decimal places)
                o.Property(p => p.TotalAmount).HasColumnType("decimal(10,2)");
                // Prevent cascade delete from App_User to Order to solve multiple cascade paths
                o.HasOne(order => order.User)
                 .WithMany(user => user.Orders)
                 .HasForeignKey(order => order.UserId)
                 .OnDelete(DeleteBehavior.NoAction);
                // Prevent cascade delete from Address to Order
                o.HasOne(order => order.Address)
                 .WithMany(address => address.Orders)
                 .HasForeignKey(order => order.ShippingAddressId)
                 .OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<Product>(p =>
            {
                p.HasIndex(i => i.SKU).IsUnique(true);
                // Configure decimal precision
                p.Property(p => p.Price).HasColumnType("decimal(10,2)");
            });
            builder.Entity<Category>(c =>
            {
                c.Property(p => p.ParentCategoryId).IsRequired(false);
                // Make the Category Name Unique
                c.HasIndex(p => p.Name).IsUnique(true);
            });
            builder.Entity<Order_Item>(oi =>
            {
                // Configure decimal precision
                oi.Property(p => p.UnitPrice).HasColumnType("decimal(10,2)");
                oi.Property(p => p.LineTotal).HasColumnType("decimal(10,2)");
            });

            // Seed Categories
            // Adding main categories and some sub-categories
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", ParentCategoryId = null },
                new Category { Id = 2, Name = "Laptops", ParentCategoryId = 1 },
                new Category { Id = 3, Name = "Smartphones", ParentCategoryId = 1 },
                new Category { Id = 4, Name = "Clothing", ParentCategoryId = null },
                new Category { Id = 5, Name = "Men's Fashion", ParentCategoryId = 4 }
            );

            // Seed Products
            // Adding a variety of products linked to the seeded categories
            builder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    CategoryId = 2, // Laptops
                    Name = "MacBook Pro 16-inch",
                    SKU = "MAC-PRO-16-2024",
                    Price = 125000.00m,     // EGP
                    Description = "Apple MacBook Pro 16-inch with M3 Max chip, 32GB RAM, and 1TB SSD. Built for Apple Intelligence.",
                    StockQuantity = 10,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1),
                    ImageUrl = "/images/products/macbookPro_16-inch.jpg" // Added image path
                },
                new Product
                {
                    Id = 2,
                    CategoryId = 2, // Laptops
                    Name = "Dell XPS 15",
                    SKU = "DELL-XPS-15-OLED",
                    Price = 95000.00m,  // EGP
                    Description = "Dell XPS 15 OLED with Intel Core i9, 32GB RAM, 1TB SSD, and NVIDIA RTX 4070. Perfect for creators.",
                    StockQuantity = 15,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 5),
                    ImageUrl = "/images/products/dell_xps15.jpg" // Added image path
                },
                new Product
                {
                    Id = 3,
                    CategoryId = 3, // Smartphones
                    Name = "iPhone 15 Pro Max",
                    SKU = "IPH-15-PM-256",
                    Price = 60000.00m, // EGP
                    Description = "Apple iPhone 15 Pro Max 256GB, Natural Titanium. Features the A17 Pro chip and a 5x Telephoto camera.",
                    StockQuantity = 25,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 2, 10),
                    ImageUrl = "/images/products/iPhone15_ProMax.jpg" // Added image path
                },
                new Product
                {
                    Id = 4,
                    CategoryId = 3, // Smartphones
                    Name = "Samsung Galaxy S24 Ultra",
                    SKU = "SAM-S24-ULT-512",
                    Price = 55000.00m,  // EGP
                    Description = "Samsung Galaxy S24 Ultra 512GB, Titanium Gray. Equipped with Galaxy AI and built-in S Pen.",
                    StockQuantity = 20,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 2, 15),
                    ImageUrl = "/images/products/samsung_galaxy_S24Ultra.jpg" // Added image path
                },
                new Product
                {
                    Id = 5,
                    CategoryId = 5, // Men's Fashion
                    Name = "Classic Cotton T-Shirt",
                    SKU = "TSHIRT-MEN-BLK-M",
                    Price = 750.00m,
                    Description = "100% premium cotton classic black t-shirt for men. Comfortable, breathable, and perfect for everyday wear.",
                    StockQuantity = 100,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 3, 1),
                    ImageUrl = "/images/products/classic_cotton_T-Shirt.jpg" // Added image path
                },
                new Product
                {
                    Id = 6,
                    CategoryId = 5, // Men's Fashion
                    Name = "Denim Jeans Relaxed Fit",
                    SKU = "JEANS-MEN-BLU-32",
                    Price = 1500.00m,
                    Description = "High-quality denim jeans, relaxed fit, classic blue wash. Durable material with standard 5-pocket styling.",
                    StockQuantity = 50,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 3, 5),
                    ImageUrl = "/images/products/denimJeans_relaxed.jpg" // Added image path
                }
            );
        }
    }
}
