using KosKu.Application.Interfaces.Repositories;
using KosKu.Domain.Entities;
using KosKu.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KosKu.Infrastructure.Repositories;

/// <summary>Implementasi repository untuk RoomType.</summary>
public class RoomTypeRepository : GenericRepository<RoomType>, IRoomTypeRepository
{
    public RoomTypeRepository(AppDbContext context) : base(context) { }

    /// <inheritdoc/>
    public async Task<RoomType?> GetWithRoomsAsync(Guid id)
        => await _dbSet.AsNoTracking()
            .Include(rt => rt.Rooms)
            .FirstOrDefaultAsync(rt => rt.Id == id);
}
