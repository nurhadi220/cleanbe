using KosKu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KosKu.Infrastructure.Persistence.Configurations;

/// <summary>Konfigurasi Fluent API untuk entity Booking.</summary>
public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("booking");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasColumnName("id");

        builder.Property(b => b.KodeBooking)
            .HasColumnName("kode_booking")
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(b => b.KodeBooking)
            .IsUnique();

        builder.Property(b => b.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(b => b.KamarId)
            .HasColumnName("kamar_id")
            .IsRequired();

        builder.Property(b => b.TanggalMasuk)
            .HasColumnName("tanggal_masuk")
            .IsRequired();

        builder.Property(b => b.DurasiBuilan)
            .HasColumnName("durasi_bulan")
            .IsRequired();

        builder.Property(b => b.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(b => b.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

        // Relationships
        builder.HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Kamar)
            .WithMany(r => r.Bookings)
            .HasForeignKey(b => b.KamarId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
