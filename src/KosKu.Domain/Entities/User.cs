using KosKu.Domain.Common;
using KosKu.Domain.Enums;

namespace KosKu.Domain.Entities;

/// <summary>Entity pengguna sistem (Admin atau Penghuni).</summary>
public class User : BaseEntity
{
    /// <summary>Nama lengkap pengguna.</summary>
    public string Nama { get; set; } = string.Empty;

    /// <summary>Alamat email unik pengguna (digunakan sebagai username).</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Password yang sudah di-hash menggunakan BCrypt.</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>Nomor handphone pengguna.</summary>
    public string? NoHp { get; set; }

    /// <summary>Peran pengguna dalam sistem.</summary>
    public UserRole Role { get; set; } = UserRole.Penghuni;

    // Navigation properties
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();
}
