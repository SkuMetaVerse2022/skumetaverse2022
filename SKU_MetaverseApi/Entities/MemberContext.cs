using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SKU_MetaverseApi.Entities
{
    public partial class MemberContext : DbContext
    {
        public MemberContext()
        {
        }

        public MemberContext(DbContextOptions<MemberContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Member> member { get; set; } = null!;
        
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;port=3306;user=seohyun;password=Blizard5000@;database=SKU_MetaVerse", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.29-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("member");

                entity.HasCharSet("utf8mb3")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.Pw).HasMaxLength(30);

                entity.Property(e => e.StudentId)
                    .HasMaxLength(30)
                    .HasColumnName("studentId");

                entity.Property(e => e.StudentMail)
                    .HasMaxLength(30)
                    .HasColumnName("studentMail");

                entity.Property(e => e.StudentName)
                    .HasMaxLength(30)
                    .HasColumnName("studentName");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
