using KosKu.Domain.Common;
using KosKu.Domain.Enums;

namespace KosKu.Domain.Entities;

/// <summary>Denda yang dikenakan kepada penghuni.</summary>
public class Fine : BaseEntity
{
    /// <summary>FK ke sewa aktif.</summary>
    public Guid SewaId { get; set; }

    /// <summary>Keterangan/alasan denda.</summary>
    public string Keterangan { get; set; } = string.Empty;

    /// <summary>Jumlah denda dalam Rupiah.</summary>
    public decimal Jumlah { get; set; }

    /// <summary>Status pembayaran denda.</summary>
    public FineStatus Status { get; set; } = FineStatus.BelumDibayar;

    // Navigation properties
    public Rental Sewa { get; set; } = null!;
}
