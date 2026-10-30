using KosKu.Application.Interfaces.Repositories;
using KosKu.Domain.Entities;
using KosKu.Domain.Enums;
using KosKu.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KosKu.Infrastructure.Repositories;

/// <summary>Implementasi repository untuk Booking.</summary>
public class BookingRepository : GenericRepository<Booking>, IBookingRepository
{
    public BookingRepository(AppDbContext context) : base(context) { }

    /// <inheritdoc/>
    public async Task<IEnumerable<Booking>> GetAllWithDetailsAsync()
        => await _dbSet.AsNoTracking()
            .Include(b => b.User)
            .Include(b => b.Kamar).ThenInclude(k => k.TipeKamar)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

    /// <inheritdoc/>
    public async Task<Booking?> GetByIdWithDetailsAsync(Guid id)
        => await _dbSet.AsNoTracking()
            .Include(b => b.User)
            .Include(b => b.Kamar).ThenInclude(k => k.TipeKamar)
            .FirstOrDefaultAsync(b => b.Id == id);

    /// <inheritdoc/>
    public async Task<Booking?> GetByKodeAsync(string kode)
        => await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(b => b.KodeBooking == kode);

    /// <inheritdoc/>
    public async Task<IEnumerable<Booking>> GetByUserIdAsync(Guid userId)
        => await _dbSet.AsNoTracking()
            .Include(b => b.Kamar).ThenInclude(k => k.TipeKamar)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

    /// <inheritdoc/>
    public async Task<int> CountPendingAsync()
        => await _dbSet.CountAsync(b => b.Status == BookingStatus.Pending);
}
