using KosKu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KosKu.Infrastructure.Persistence.Configurations;

/// <summary>Konfigurasi Fluent API untuk entity Fine (denda).</summary>
public class FineConfiguration : IEntityTypeConfiguration<Fine>
{
    public void Configure(EntityTypeBuilder<Fine> builder)
    {
        builder.ToTable("denda");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .HasColumnName("id");

        builder.Property(f => f.SewaId)
            .HasColumnName("sewa_id")
            .IsRequired();

        builder.Property(f => f.Keterangan)
            .HasColumnName("keterangan")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(f => f.Jumlah)
            .HasColumnName("jumlah")
            .HasPrecision(15, 2)
            .IsRequired();

        builder.Property(f => f.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(f => f.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

        // Relationships
        builder.HasOne(f => f.Sewa)
            .WithMany(r => r.Fines)
            .HasForeignKey(f => f.SewaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
