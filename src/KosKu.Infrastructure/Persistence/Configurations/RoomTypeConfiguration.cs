using KosKu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KosKu.Infrastructure.Persistence.Configurations;

/// <summary>Konfigurasi Fluent API untuk entity RoomType.</summary>
public class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
{
    public void Configure(EntityTypeBuilder<RoomType> builder)
    {
        builder.ToTable("tipe_kamar");

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Id)
            .HasColumnName("id");

        builder.Property(rt => rt.NamaTipe)
            .HasColumnName("nama_tipe")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(rt => rt.Deskripsi)
            .HasColumnName("deskripsi")
            .HasMaxLength(500);

        builder.Property(rt => rt.HargaPerBulan)
            .HasColumnName("harga_per_bulan")
            .HasPrecision(15, 2)
            .IsRequired();

        builder.Property(rt => rt.Fasilitas)
            .HasColumnName("fasilitas")
            .HasColumnType("longtext");

        builder.Property(rt => rt.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
    }
}
