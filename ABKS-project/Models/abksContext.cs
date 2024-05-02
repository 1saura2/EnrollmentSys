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

        public virtual DbSet<Batch> Batches { get; set; } = null!;
        public virtual DbSet<Credential> Credentials { get; set; } = null!;
        public virtual DbSet<Evaluation> Evaluations { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserBatch> UserBatches { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Batch>(entity =>
            {
                entity.Property(e => e.BatchName).HasMaxLength(50);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            modelBuilder.Entity<Credential>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.Token).HasMaxLength(100);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Credentials)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Credentia__RoleI__6E01572D");
            });

            modelBuilder.Entity<Evaluation>(entity =>
            {
                entity.Property(e => e.Attendance).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.DisciplineTest).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.EvaluationDate).HasColumnType("date");

                entity.Property(e => e.FitnessTest).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.WriteTest).HasColumnType("decimal(5, 2)");

                entity.HasOne(d => d.UserBatch)
                    .WithMany(p => p.Evaluations)
                    .HasForeignKey(d => d.UserBatchId)
                    .HasConstraintName("FK__Evaluatio__UserB__797309D9");
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

            modelBuilder.Entity<UserBatch>(entity =>
            {
                entity.HasIndex(e => new { e.UserId, e.BatchId }, "UQ_User_Batch")
                    .IsUnique();

                entity.HasOne(d => d.Batch)
                    .WithMany(p => p.UserBatches)
                    .HasForeignKey(d => d.BatchId)
                    .HasConstraintName("FK__UserBatch__Batch__76969D2E");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserBatches)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__UserBatch__UserI__75A278F5");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
