﻿using EAD_Backend_Application__.NET.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EAD_Backend_Application__.NET.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        // Command to generate a new migration based on the current model schema.
        // Example usage:
        // Add-Migration InitialCreate -OutputDir Data/Migrations

        // Command to apply any pending migrations to the database and update its schema.
        // Example usage:
        // Update-Database

        // Command to undo the last migration if it has not yet been applied to the database.
        // Example usage:
        // Remove-Migration

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // List of necessary packages to install for database and identity functionality:
        // 1. Microsoft.EntityFrameworkCore.SqlServer
        //    - Provides SQL Server support for Entity Framework Core.
        //    - Installation: dotnet add package Microsoft.EntityFrameworkCore.SqlServer

        // 2. Microsoft.EntityFrameworkCore.Tools
        //    - Provides tools for Entity Framework Core, including migrations.
        //    - Installation: dotnet add package Microsoft.EntityFrameworkCore.Tools

        // 3. Microsoft.AspNetCore.Identity.EntityFrameworkCore
        //    - Provides ASP.NET Core Identity support using Entity Framework Core.
        //    - Installation: dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore

        // 4. Microsoft.AspNetCore.Authentication.JwtBearer
        //    - Provides support for JSON Web Token (JWT) bearer authentication.
        //    - Installation: dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer

        // 5. System.IdentityModel.Tokens.Jwt
        //    - Handles JWT security tokens and token validation.
        //    - Installation: dotnet add package System.IdentityModel.Tokens.Jwt

        // 6. Swashbuckle.AspNetCore
        //    - Adds Swagger for API documentation and UI in ASP.NET Core.
        //    - Installation: dotnet add package Swashbuckle.AspNetCore

        // 7. Microsoft.AspNetCore.Mvc.NewtonsoftJson
        //    - Adds support for JSON serialization using Newtonsoft.Json in ASP.NET Core MVC.
        //    - Installation: dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson

        // 8. Microsoft.Extensions.Configuration.Json
        //    - Provides configuration support using JSON files (like appsettings.json).
        //    - Installation: dotnet add package Microsoft.Extensions.Configuration.Json

        public DbSet<UserModel> Users { get; set; } = default!;
        public DbSet<ProductModel> Products { get; set; } = default!;
        public DbSet<ProductColor> ProductColors { get; set; } = default!;
        public DbSet<ProductSize> ProductSizes { get; set; } = default!;
        public DbSet<CartItemModel> CartItems { get; set; } = default!;
        public DbSet<OrderModel> Orders { get; set; } = default!;
        public DbSet<OrderItemModel> OrderItems { get; set; } = default!;
        public DbSet<RankingModel> Rankings { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // CONFIGURE USER MODEL
            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.ToTable("Users");

                // CONFIGURE FOREIGN KEY RELATIONSHIP WITH PRODUCTS
                entity.HasMany(u => u.Products)
                  .WithOne(p => p.Vendor) 
                  .HasForeignKey(p => p.VendorId)
                  .OnDelete(DeleteBehavior.Restrict);

                // CONFIGURE RELATIONSHIP WITH CART ITEMS
                entity.HasMany(u => u.CartItems)
                      .WithOne(c => c.User)
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // CONFIGURE RELATIONSHIP WITH ORDERS
                entity.HasMany(u => u.Orders)
                      .WithOne(o => o.User)
                      .HasForeignKey(o => o.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade);

                // CONFIGURE RELATIONSHIP WITH RANKINGS
                entity.HasMany(u => u.Rankings)
                      .WithOne(r => r.Vendor)
                      .HasForeignKey(r => r.VendorId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // CONFIGURE PRODUCT MODEL
            modelBuilder.Entity<ProductModel>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(p => p.ProductId);

                // CONFIGURE ONE-TO-MANY RELATIONSHIP WITH PRODUCT-COLOR
                entity.HasMany(p => p.Colors)
                      .WithOne(c => c.Product)
                      .HasForeignKey(c => c.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                // CONFIGURE ONE-TO-MANY RELATIONSHIP WITH PRODUCT-SIZE
                entity.HasMany(p => p.Sizes)
                      .WithOne(s => s.Product)
                      .HasForeignKey(s => s.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                // CONFIGURE RELATIONSHIP WITH CART ITEMS
                entity.HasMany(p => p.CartItems)
                      .WithOne(c => c.Product)
                      .HasForeignKey(c => c.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // CONFIGURE PRODUCT COLOR
            modelBuilder.Entity<ProductColor>(entity =>
            {
                entity.ToTable("ProductColors");
                entity.HasKey(c => c.ColorId);
            });

            // CONFIGURE PRODUCT SIZE
            modelBuilder.Entity<ProductSize>(entity =>
            {
                entity.ToTable("ProductSizes");
                entity.HasKey(s => s.SizeId);
            });

            // CONFIGURE CART MODEL
            modelBuilder.Entity<CartItemModel>(entity =>
            {
                entity.ToTable("CartItems");
                entity.HasKey(c => c.CartId);
            });

            // CONFIGURE ORDER MODEL
            modelBuilder.Entity<OrderModel>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(o => o.OrderId);

                // CONFIGURE RELATIONSHIP WITH ORDER ITEMS
                entity.HasMany(o => o.OrderItems)
                      .WithOne(oi => oi.Order)
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // CONFIGURE ORDER DETAILS MODEL
            modelBuilder.Entity<OrderItemModel>(entity =>
            {
                entity.ToTable("OrderItems");
                entity.HasKey(oi => oi.OrderItemId);
            });

            // CONFIGURE RANKING MODEL
            modelBuilder.Entity<RankingModel>(entity =>
            {
                entity.ToTable("Rankings");
                entity.HasKey(r => r.RankingId);
            });
        }

    }
}
