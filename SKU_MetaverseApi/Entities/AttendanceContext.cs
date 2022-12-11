using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SKU_MetaverseApi.Entities
{
    public partial class AttendanceContext : DbContext
    {
        public AttendanceContext()
        {

        }

        public AttendanceContext(DbContextOptions<AttendanceContext> options) : base(options)
        {

        }

        public virtual DbSet<Attendance> attendance { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;port=3306;user=seohyun;password=Blizard5000@;database=SKU_MetaVerse", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.29-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci").HasCharSet("utf8mb4");

            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.ToTable("Attendance");
                entity.HasCharSet("utf8mb3").UseCollation("utf8_general_ci");
                entity.Property(e => e.aStudentId).HasMaxLength(30).HasColumnName("studentId");
                entity.Property(e => e.aStudentName).HasMaxLength(30).HasColumnName("studentName");
                entity.Property(e => e.aAttendTime).HasMaxLength(30).HasColumnName("attendLog");
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
