using KosKu.Application.Interfaces.Repositories;
using KosKu.Domain.Entities;
using KosKu.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KosKu.Infrastructure.Repositories;

/// <summary>Implementasi repository untuk MaintenanceRequest (perbaikan_layanan).</summary>
public class MaintenanceRepository : GenericRepository<MaintenanceRequest>, IMaintenanceRepository
{
    public MaintenanceRepository(AppDbContext context) : base(context) { }

    /// <inheritdoc/>
    public async Task<IEnumerable<MaintenanceRequest>> GetAllWithDetailsAsync()
        => await _dbSet.AsNoTracking()
            .Include(m => m.User)
            .Include(m => m.Kamar)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

    /// <inheritdoc/>
    public async Task<IEnumerable<MaintenanceRequest>> GetByRoomIdAsync(Guid kamarId)
        => await _dbSet.AsNoTracking()
            .Include(m => m.User)
            .Where(m => m.KamarId == kamarId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

    /// <inheritdoc/>
    public async Task<IEnumerable<MaintenanceRequest>> GetByUserIdAsync(Guid userId)
        => await _dbSet.AsNoTracking()
            .Include(m => m.Kamar)
            .Where(m => m.UserId == userId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
}
