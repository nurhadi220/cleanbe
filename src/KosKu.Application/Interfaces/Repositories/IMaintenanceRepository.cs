using KosKu.Domain.Entities;

namespace KosKu.Application.Interfaces.Repositories;

/// <summary>Repository khusus untuk entity MaintenanceRequest (perbaikan_layanan).</summary>
public interface IMaintenanceRepository : IGenericRepository<MaintenanceRequest>
{
    /// <summary>Mendapatkan laporan dengan detail kamar dan user.</summary>
    Task<IEnumerable<MaintenanceRequest>> GetAllWithDetailsAsync();

    /// <summary>Mendapatkan laporan berdasarkan kamar.</summary>
    Task<IEnumerable<MaintenanceRequest>> GetByRoomIdAsync(Guid kamarId);

    /// <summary>Mendapatkan laporan berdasarkan user.</summary>
    Task<IEnumerable<MaintenanceRequest>> GetByUserIdAsync(Guid userId);
}
