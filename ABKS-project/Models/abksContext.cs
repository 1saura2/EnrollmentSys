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

        public virtual DbSet<Attendance> Attendances { get; set; } = null!;
        public virtual DbSet<Batch> Batches { get; set; } = null!;
        public virtual DbSet<Credential> Credentials { get; set; } = null!;
        public virtual DbSet<Evaluation> Evaluations { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserBatch> UserBatches { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            { }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.Property(e => e.AttendanceDate).HasColumnType("date");

                entity.HasOne(d => d.UserBatch)
                    .WithMany(p => p.Attendances)
                    .HasForeignKey(d => d.UserBatchId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Attendances_UserBatch");
            });

            modelBuilder.Entity<Batch>(entity =>
            {
                entity.Property(e => e.BatchName).HasMaxLength(100);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            modelBuilder.Entity<Credential>(entity =>
            {
                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.Token).HasMaxLength(100);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Credentials)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Credentia__RoleI__6442E2C9");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Credentials)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Credentials_Users");
            });

            modelBuilder.Entity<Evaluation>(entity =>
            {
                entity.Property(e => e.DisciplineTest).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.EvaluationDate).HasColumnType("date");

                entity.Property(e => e.FitnessTest).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.WriteTest).HasColumnType("decimal(5, 2)");

                entity.HasOne(d => d.UserBatch)
                    .WithMany(p => p.Evaluations)
                    .HasForeignKey(d => d.UserBatchId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Evaluations_UserBatch");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ContactNumber).HasMaxLength(20);

                entity.Property(e => e.Education).HasMaxLength(100);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);
            });

            modelBuilder.Entity<UserBatch>(entity =>
            {
                entity.ToTable("UserBatch");

                entity.HasOne(d => d.Batch)
                    .WithMany(p => p.UserBatches)
                    .HasForeignKey(d => d.BatchId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_UserBatch_Batches");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserBatches)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_UserBatch_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
