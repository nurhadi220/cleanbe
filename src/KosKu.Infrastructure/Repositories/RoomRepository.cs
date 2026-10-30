using KosKu.Application.Interfaces.Repositories;
using KosKu.Domain.Entities;
using KosKu.Domain.Enums;
using KosKu.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KosKu.Infrastructure.Repositories;

/// <summary>Implementasi repository untuk Room.</summary>
public class RoomRepository : GenericRepository<Room>, IRoomRepository
{
    public RoomRepository(AppDbContext context) : base(context) { }

    /// <inheritdoc/>
    public async Task<IEnumerable<Room>> GetAllWithTypeAsync()
        => await _dbSet.AsNoTracking()
            .Include(r => r.TipeKamar)
            .OrderBy(r => r.NomorKamar)
            .ToListAsync();

    /// <inheritdoc/>
    public async Task<Room?> GetByIdWithTypeAsync(Guid id)
        => await _dbSet.AsNoTracking()
            .Include(r => r.TipeKamar)
            .FirstOrDefaultAsync(r => r.Id == id);

    /// <inheritdoc/>
    public async Task<IEnumerable<Room>> GetByStatusAsync(RoomStatus status)
        => await _dbSet.AsNoTracking()
            .Include(r => r.TipeKamar)
            .Where(r => r.Status == status)
            .ToListAsync();

    /// <inheritdoc/>
    public async Task<int> CountTotalAsync()
        => await _dbSet.CountAsync();
}
