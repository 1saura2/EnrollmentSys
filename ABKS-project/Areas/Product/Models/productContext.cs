using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ABKS_project.Areas.Product.Models
{
    public partial class productContext : DbContext
    {
        public productContext()
        {
        }

        public productContext(DbContextOptions<productContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CartDetail> CartDetails { get; set; } = null!;
        public virtual DbSet<ErrorViewModel> ErrorViewModels { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductCategory> ProductCategories { get; set; } = null!;
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; } = null!;
        public virtual DbSet<Stock> Stocks { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=103.182.175.232,1433;Initial Catalog=abks-project;Integrated Security=False;Persist Security Info=False;User ID=intern;Password=intern@123;Connect Timeout=60");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartDetail>(entity =>
            {
                entity.HasKey(e => e.CartId)
                    .HasName("PK__CartDeta__51BCD7B7A5742805");

                entity.ToTable("CartDetail");

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<ErrorViewModel>(entity =>
            {
                entity.HasKey(e => e.RequestId)
                    .HasName("PK__ErrorVie__33A8517A031AF941");

                entity.ToTable("ErrorViewModel");

                entity.Property(e => e.RequestId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.OrderEmail).HasMaxLength(255);

                entity.Property(e => e.OrderMobNumber).HasMaxLength(20);

                entity.Property(e => e.OrderName).HasMaxLength(255);

                entity.Property(e => e.OrderPaymentMethod).HasMaxLength(100);
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.ProductName).HasMaxLength(255);

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("PK__OrderSta__C8EE2063520AAF60");

                entity.ToTable("OrderStatus");

                entity.Property(e => e.StatusId).ValueGeneratedNever();

                entity.Property(e => e.StatusName).HasMaxLength(100);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.ProductImage).HasMaxLength(255);

                entity.Property(e => e.ProductName).HasMaxLength(255);

                entity.Property(e => e.ProductPrice).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.ToTable("ProductCategory");

                entity.Property(e => e.ProductCategoryId)
                    .ValueGeneratedNever()
                    .HasColumnName("ProductCategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(100);
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.ToTable("ShoppingCart");
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.ToTable("Stock");

                entity.Property(e => e.StockId).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
