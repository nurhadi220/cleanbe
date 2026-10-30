using KosKu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KosKu.Infrastructure.Persistence.Configurations;

/// <summary>Konfigurasi Fluent API untuk entity MaintenanceRequest (perbaikan_layanan).</summary>
public class MaintenanceRequestConfiguration : IEntityTypeConfiguration<MaintenanceRequest>
{
    public void Configure(EntityTypeBuilder<MaintenanceRequest> builder)
    {
        builder.ToTable("perbaikan_layanan");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("id");

        builder.Property(m => m.KamarId)
            .HasColumnName("kamar_id")
            .IsRequired();

        builder.Property(m => m.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(m => m.JudulLaporan)
            .HasColumnName("judul_laporan")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(m => m.Deskripsi)
            .HasColumnName("deskripsi")
            .HasColumnType("longtext")
            .IsRequired();

        builder.Property(m => m.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(m => m.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

        // Relationships
        builder.HasOne(m => m.Kamar)
            .WithMany(r => r.MaintenanceRequests)
            .HasForeignKey(m => m.KamarId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.User)
            .WithMany(u => u.MaintenanceRequests)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
