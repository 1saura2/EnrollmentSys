using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ABKS_project.Areas.Product.Models.DTOs
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

        public virtual DbSet<CheckoutModel> CheckoutModels { get; set; } = null!;
        public virtual DbSet<OrderDetailModelDto> OrderDetailModelDtos { get; set; } = null!;
        public virtual DbSet<ProductCategoryDto> ProductCategoryDtos { get; set; } = null!;
        public virtual DbSet<ProductDisplayModel> ProductDisplayModels { get; set; } = null!;
        public virtual DbSet<ProductDto> ProductDtos { get; set; } = null!;
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
            modelBuilder.Entity<CheckoutModel>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CheckoutModel");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.FirstName).HasMaxLength(255);

                entity.Property(e => e.LastName).HasMaxLength(255);

                entity.Property(e => e.MobileNumber).HasMaxLength(20);

                entity.Property(e => e.PaymentMethod).HasMaxLength(100);
            });

            modelBuilder.Entity<OrderDetailModelDto>(entity =>
            {
                entity.HasKey(e => e.Obmdid)
                    .HasName("PK__OrderDet__4972CC45E029B1FD");

                entity.ToTable("OrderDetailModelDTO");

                entity.Property(e => e.Obmdid).HasColumnName("OBMDId");
            });

            modelBuilder.Entity<ProductCategoryDto>(entity =>
            {
                entity.HasKey(e => e.ProductCategoryId)
                    .HasName("PK__ProductC__3224ECCE447FBE5E");

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

            modelBuilder.Entity<StockDisplayModelDto>(entity =>
            {
                entity.HasKey(e => e.StockDtoid)
                    .HasName("PK__StockDis__F31831C8A9EDBF62");

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
