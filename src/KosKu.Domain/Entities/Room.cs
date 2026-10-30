using KosKu.Domain.Common;
using KosKu.Domain.Enums;

namespace KosKu.Domain.Entities;

/// <summary>Unit kamar kos.</summary>
public class Room : BaseEntity
{
    /// <summary>Nomor kamar (misal: A-101).</summary>
    public string NomorKamar { get; set; } = string.Empty;

    /// <summary>Lantai tempat kamar berada.</summary>
    public int Lantai { get; set; }

    /// <summary>FK ke tipe kamar.</summary>
    public Guid TipeKamarId { get; set; }

    /// <summary>Status ketersediaan kamar saat ini.</summary>
    public RoomStatus Status { get; set; } = RoomStatus.Tersedia;

    /// <summary>URL foto kamar (opsional).</summary>
    public string? FotoUrl { get; set; }

    // Navigation properties
    public RoomType TipeKamar { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    public ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();
}
