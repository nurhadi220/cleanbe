using KosKu.Domain.Common;

namespace KosKu.Domain.Entities;

/// <summary>Tipe/kategori kamar kos (misal: Standard, Deluxe, VIP).</summary>
public class RoomType : BaseEntity
{
    /// <summary>Nama tipe kamar.</summary>
    public string NamaTipe { get; set; } = string.Empty;

    /// <summary>Deskripsi tipe kamar.</summary>
    public string? Deskripsi { get; set; }

    /// <summary>Harga sewa per bulan dalam Rupiah.</summary>
    public decimal HargaPerBulan { get; set; }

    /// <summary>Daftar fasilitas yang tersedia (JSON string atau comma-separated).</summary>
    public string? Fasilitas { get; set; }

    // Navigation properties
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}
