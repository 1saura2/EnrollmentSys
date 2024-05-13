using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ABKS_project.Models
{
    public partial class abksContext : DbContext
    {
        public abksContext()
        {
        }

        public abksContext(DbContextOptions<abksContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CartDetail> CartDetails { get; set; } = null!;
        public virtual DbSet<Credential> Credentials { get; set; } = null!;
        public virtual DbSet<ErrorViewModel> ErrorViewModels { get; set; } = null!;
        public virtual DbSet<Evaluation> Evaluations { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductCategory> ProductCategories { get; set; } = null!;
        public virtual DbSet<RegistrationType> RegistrationTypes { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; } = null!;
        public virtual DbSet<Stock> Stocks { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRegistrationType> UserRegistrationTypes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
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

            modelBuilder.Entity<Credential>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.Token).HasMaxLength(100);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Credentials)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Credentia__RoleI__2EDAF651");
            });

            modelBuilder.Entity<ErrorViewModel>(entity =>
            {
                entity.HasKey(e => e.RequestId)
                    .HasName("PK__ErrorVie__33A8517A031AF941");

                entity.ToTable("ErrorViewModel");

                entity.Property(e => e.RequestId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Evaluation>(entity =>
            {
                entity.Property(e => e.Attendance).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.DisciplineTest).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.EvaluationDate).HasColumnType("date");

                entity.Property(e => e.FitnessTest).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.WriteTest).HasColumnType("decimal(5, 2)");

                entity.HasOne(d => d.UserRegistrationType)
                    .WithMany(p => p.Evaluations)
                    .HasForeignKey(d => d.UserRegistrationTypeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Evaluatio__UserR__37703C52");
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

            modelBuilder.Entity<RegistrationType>(entity =>
            {
                entity.Property(e => e.TypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleName).HasMaxLength(50);
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

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.ContactNumber).HasMaxLength(20);

                entity.Property(e => e.Education).HasMaxLength(100);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FullName).HasMaxLength(100);
            });

            modelBuilder.Entity<UserRegistrationType>(entity =>
            {
                entity.HasOne(d => d.RegistrationType)
                    .WithMany(p => p.UserRegistrationTypes)
                    .HasForeignKey(d => d.RegistrationTypeId)
                    .HasConstraintName("FK__UserRegis__Regis__3493CFA7");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRegistrationTypes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__UserRegis__UserI__339FAB6E");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
