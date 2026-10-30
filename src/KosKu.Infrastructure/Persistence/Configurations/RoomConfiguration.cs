using KosKu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KosKu.Infrastructure.Persistence.Configurations;

/// <summary>Konfigurasi Fluent API untuk entity Room.</summary>
public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("kamar");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName("id");

        builder.Property(r => r.NomorKamar)
            .HasColumnName("nomor_kamar")
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(r => r.NomorKamar)
            .IsUnique();

        builder.Property(r => r.Lantai)
            .HasColumnName("lantai")
            .IsRequired();

        builder.Property(r => r.TipeKamarId)
            .HasColumnName("tipe_kamar_id")
            .IsRequired();

        builder.Property(r => r.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(r => r.FotoUrl)
            .HasColumnName("foto_url")
            .HasColumnType("TEXT");
        builder.Property(r => r.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

        // Relationships
        builder.HasOne(r => r.TipeKamar)
            .WithMany(rt => rt.Rooms)
            .HasForeignKey(r => r.TipeKamarId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
