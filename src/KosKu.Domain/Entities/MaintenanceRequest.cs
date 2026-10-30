using KosKu.Domain.Common;
using KosKu.Domain.Enums;

namespace KosKu.Domain.Entities;

/// <summary>Laporan permintaan perbaikan/maintenance dari penghuni.</summary>
public class MaintenanceRequest : BaseEntity
{
    /// <summary>FK ke kamar yang mengajukan perbaikan.</summary>
    public Guid KamarId { get; set; }

    /// <summary>FK ke penghuni yang melaporkan.</summary>
    public Guid UserId { get; set; }

    /// <summary>Judul singkat laporan.</summary>
    public string JudulLaporan { get; set; } = string.Empty;

    /// <summary>Deskripsi detail kerusakan atau permintaan.</summary>
    public string Deskripsi { get; set; } = string.Empty;

    /// <summary>Status penanganan laporan.</summary>
    public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Pending;

    // Navigation properties
    public Room Kamar { get; set; } = null!;
    public User User { get; set; } = null!;
}
