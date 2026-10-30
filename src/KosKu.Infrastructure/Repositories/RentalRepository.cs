using KosKu.Application.Interfaces.Repositories;
using KosKu.Domain.Entities;
using KosKu.Domain.Enums;
using KosKu.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KosKu.Infrastructure.Repositories;

/// <summary>Implementasi repository untuk Rental (sewa aktif).</summary>
public class RentalRepository : GenericRepository<Rental>, IRentalRepository
{
    public RentalRepository(AppDbContext context) : base(context) { }

    /// <inheritdoc/>
    public async Task<IEnumerable<Rental>> GetAllWithDetailsAsync()
        => await _dbSet.AsNoTracking()
            .Include(r => r.User)
            .Include(r => r.Kamar).ThenInclude(k => k.TipeKamar)
            .Include(r => r.Booking)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

    /// <inheritdoc/>
    public async Task<Rental?> GetByIdWithDetailsAsync(Guid id)
        => await _dbSet.AsNoTracking()
            .Include(r => r.User)
            .Include(r => r.Kamar).ThenInclude(k => k.TipeKamar)
            .Include(r => r.Booking)
            .Include(r => r.Payments)
            .Include(r => r.Fines)
            .FirstOrDefaultAsync(r => r.Id == id);

    /// <inheritdoc/>
    public async Task<IEnumerable<Rental>> GetByUserIdAsync(Guid userId)
        => await _dbSet.AsNoTracking()
            .Include(r => r.Kamar).ThenInclude(k => k.TipeKamar)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

    /// <inheritdoc/>
    public async Task<Rental?> GetActiveByRoomIdAsync(Guid kamarId)
        => await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(r => r.KamarId == kamarId && r.Status == RentalStatus.Aktif);

    /// <inheritdoc/>
    public async Task<int> CountActiveAsync()
        => await _dbSet.CountAsync(r => r.Status == RentalStatus.Aktif);
}
