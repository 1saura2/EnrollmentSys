using System;
using System.Collections.Generic;
using ABKS_project.Models.EcommerceContent;
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

        public virtual DbSet<Credential> Credentials { get; set; } = null!;
        public virtual DbSet<Evaluation> Evaluations { get; set; } = null!;
        public virtual DbSet<RegistrationType> RegistrationTypes { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRegistrationType> UserRegistrationTypes { get; set; } = null!;
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> orderStatuses { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            { }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<RegistrationType>(entity =>
            {
                entity.Property(e => e.TypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleName).HasMaxLength(50);
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
