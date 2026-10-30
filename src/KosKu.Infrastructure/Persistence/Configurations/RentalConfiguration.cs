using KosKu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KosKu.Infrastructure.Persistence.Configurations;

/// <summary>Konfigurasi Fluent API untuk entity Rental (sewa_aktif).</summary>
public class RentalConfiguration : IEntityTypeConfiguration<Rental>
{
    public void Configure(EntityTypeBuilder<Rental> builder)
    {
        builder.ToTable("sewa_aktif");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName("id");

        builder.Property(r => r.BookingId)
            .HasColumnName("booking_id")
            .IsRequired();

        builder.Property(r => r.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(r => r.KamarId)
            .HasColumnName("kamar_id")
            .IsRequired();

        builder.Property(r => r.TanggalMulai)
            .HasColumnName("tanggal_mulai")
            .IsRequired();

        builder.Property(r => r.TanggalSelesai)
            .HasColumnName("tanggal_selesai")
            .IsRequired();

        builder.Property(r => r.JatuhTempoBerikutnya)
            .HasColumnName("jatuh_tempo_berikutnya")
            .IsRequired();

        builder.Property(r => r.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(r => r.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

        // Relationships
        builder.HasOne(r => r.Booking)
            .WithOne(b => b.Rental)
            .HasForeignKey<Rental>(r => r.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.User)
            .WithMany(u => u.Rentals)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Kamar)
            .WithMany(k => k.Rentals)
            .HasForeignKey(r => r.KamarId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
