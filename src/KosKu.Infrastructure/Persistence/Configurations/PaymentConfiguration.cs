using KosKu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KosKu.Infrastructure.Persistence.Configurations;

/// <summary>Konfigurasi Fluent API untuk entity Payment (pembayaran).</summary>
public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("pembayaran");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.SewaId)
            .HasColumnName("sewa_id")
            .IsRequired();

        builder.Property(p => p.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(p => p.Jumlah)
            .HasColumnName("jumlah")
            .HasPrecision(15, 2)
            .IsRequired();

        builder.Property(p => p.TanggalBayar)
            .HasColumnName("tanggal_bayar")
            .IsRequired();

        builder.Property(p => p.MetodePembayaran)
            .HasColumnName("metode_pembayaran")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.StatusPembayaran)
            .HasColumnName("status_pembayaran")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.BuktiPembayaran)
            .HasColumnName("bukti_pembayaran")
            .HasMaxLength(500);

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

        // Relationships
        builder.HasOne(p => p.Sewa)
            .WithMany(r => r.Payments)
            .HasForeignKey(p => p.SewaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.User)
            .WithMany(u => u.Payments)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
