using KosKu.Domain.Common;
using KosKu.Domain.Enums;

namespace KosKu.Domain.Entities;

/// <summary>Permintaan booking kamar oleh penghuni.</summary>
public class Booking : BaseEntity
{
    /// <summary>Kode booking unik yang ditampilkan ke pengguna.</summary>
    public string KodeBooking { get; set; } = string.Empty;

    /// <summary>FK ke pengguna yang melakukan booking.</summary>
    public Guid UserId { get; set; }

    /// <summary>FK ke kamar yang dipesan.</summary>
    public Guid KamarId { get; set; }

    /// <summary>Tanggal rencana masuk.</summary>
    public DateOnly TanggalMasuk { get; set; }

    /// <summary>Durasi sewa dalam bulan.</summary>
    public int DurasiBuilan { get; set; }

    /// <summary>Status booking saat ini.</summary>
    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    // Navigation properties
    public User User { get; set; } = null!;
    public Room Kamar { get; set; } = null!;
    public Rental? Rental { get; set; }
}
