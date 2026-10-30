using KosKu.Domain.Common;
using KosKu.Domain.Enums;

namespace KosKu.Domain.Entities;

/// <summary>Riwayat pembayaran sewa.</summary>
public class Payment : BaseEntity
{
    /// <summary>FK ke sewa aktif.</summary>
    public Guid SewaId { get; set; }

    /// <summary>FK ke pengguna yang membayar.</summary>
    public Guid UserId { get; set; }

    /// <summary>Jumlah pembayaran dalam Rupiah.</summary>
    public decimal Jumlah { get; set; }

    /// <summary>Tanggal dan waktu pembayaran dilakukan.</summary>
    public DateTime TanggalBayar { get; set; }

    /// <summary>Metode pembayaran (Transfer, Cash, dll).</summary>
    public string MetodePembayaran { get; set; } = string.Empty;

    /// <summary>Status pembayaran.</summary>
    public PaymentStatus StatusPembayaran { get; set; } = PaymentStatus.Pending;

    /// <summary>URL bukti pembayaran (opsional).</summary>
    public string? BuktiPembayaran { get; set; }

    // Navigation properties
    public Rental Sewa { get; set; } = null!;
    public User User { get; set; } = null!;
}
