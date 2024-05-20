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
        public virtual DbSet<CheckoutModel> CheckoutModels { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<OrderDetailModelDto> OrderDetailModelDtos { get; set; } = null!;
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductCategory> ProductCategories { get; set; } = null!;
        public virtual DbSet<ProductCategoryDto> ProductCategoryDtos { get; set; } = null!;
        public virtual DbSet<ProductDisplayModel> ProductDisplayModels { get; set; } = null!;
        public virtual DbSet<ProductDto> ProductDtos { get; set; } = null!;
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; } = null!;
        public virtual DbSet<Stock> Stocks { get; set; } = null!;
        public virtual DbSet<StockDisplayModelDto> StockDisplayModelDtos { get; set; } = null!;
        public virtual DbSet<StockDto> StockDtos { get; set; } = null!;
        public virtual DbSet<UpdateOrderStatusModel> UpdateOrderStatusModels { get; set; } = null!;

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

            modelBuilder.Entity<CheckoutModel>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CheckoutModel");

                entity.Property(e => e.ContactNumber).HasMaxLength(20);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.FirstName).HasMaxLength(255);

                entity.Property(e => e.LastName).HasMaxLength(255);

                entity.Property(e => e.PaymentMethod).HasMaxLength(100);
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

            modelBuilder.Entity<OrderDetailModelDto>(entity =>
            {
                entity.HasKey(e => e.Obmdid)
                    .HasName("PK__OrderDet__4972CC45BC68088E");

                entity.ToTable("OrderDetailModelDTO");

                entity.Property(e => e.Obmdid).HasColumnName("OBMDId");
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

            modelBuilder.Entity<ProductCategoryDto>(entity =>
            {
                entity.HasKey(e => e.ProductCategoryId)
                    .HasName("PK__ProductC__3224ECCE6799E607");

                entity.ToTable("ProductCategoryDTO");

                entity.Property(e => e.CategoryName).HasMaxLength(255);
            });

            modelBuilder.Entity<ProductDisplayModel>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ProductDisplayModel");

                entity.Property(e => e.Sterm)
                    .HasMaxLength(255)
                    .HasColumnName("STerm");
            });

            modelBuilder.Entity<ProductDto>(entity =>
            {
                entity.ToTable("ProductDTO");

                entity.Property(e => e.ProductDtoid).HasColumnName("ProductDTOId");

                entity.Property(e => e.ProductImage).HasMaxLength(255);

                entity.Property(e => e.ProductName).HasMaxLength(255);

                entity.Property(e => e.ProductPrice).HasColumnType("decimal(18, 2)");
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

            modelBuilder.Entity<StockDisplayModelDto>(entity =>
            {
                entity.HasKey(e => e.StockDtoid)
                    .HasName("PK__StockDis__F31831C8EB2972FA");

                entity.ToTable("StockDisplayModelDTO");

                entity.Property(e => e.StockDtoid).HasColumnName("StockDTOId");

                entity.Property(e => e.ProductName).HasMaxLength(255);
            });

            modelBuilder.Entity<StockDto>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("StockDTO");
            });

            modelBuilder.Entity<UpdateOrderStatusModel>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("UpdateOrderStatusModel");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
