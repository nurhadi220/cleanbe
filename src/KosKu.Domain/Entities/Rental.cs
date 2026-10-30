using KosKu.Domain.Common;
using KosKu.Domain.Enums;

namespace KosKu.Domain.Entities;

/// <summary>Sewa aktif setelah booking dikonfirmasi.</summary>
public class Rental : BaseEntity
{
    /// <summary>FK ke booking yang menghasilkan sewa ini.</summary>
    public Guid BookingId { get; set; }

    /// <summary>FK ke penghuni.</summary>
    public Guid UserId { get; set; }

    /// <summary>FK ke kamar yang disewa.</summary>
    public Guid KamarId { get; set; }

    /// <summary>Tanggal mulai sewa.</summary>
    public DateOnly TanggalMulai { get; set; }

    /// <summary>Tanggal selesai sewa.</summary>
    public DateOnly TanggalSelesai { get; set; }

    /// <summary>Tanggal jatuh tempo pembayaran berikutnya.</summary>
    public DateOnly JatuhTempoBerikutnya { get; set; }

    /// <summary>Status sewa saat ini.</summary>
    public RentalStatus Status { get; set; } = RentalStatus.Aktif;

    // Navigation properties
    public Booking Booking { get; set; } = null!;
    public User User { get; set; } = null!;
    public Room Kamar { get; set; } = null!;
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<Fine> Fines { get; set; } = new List<Fine>();
}
