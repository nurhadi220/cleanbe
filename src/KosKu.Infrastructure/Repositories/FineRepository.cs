using KosKu.Application.Interfaces.Repositories;
using KosKu.Domain.Entities;
using KosKu.Domain.Enums;
using KosKu.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KosKu.Infrastructure.Repositories;

/// <summary>Implementasi repository untuk Fine (denda).</summary>
public class FineRepository : GenericRepository<Fine>, IFineRepository
{
    public FineRepository(AppDbContext context) : base(context) { }

    /// <inheritdoc/>
    public async Task<IEnumerable<Fine>> GetByRentalIdAsync(Guid sewaId)
        => await _dbSet.AsNoTracking()
            .Where(f => f.SewaId == sewaId)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();

    /// <inheritdoc/>
    public async Task<decimal> GetTotalUnpaidAsync()
        => await _dbSet
            .Where(f => f.Status == FineStatus.BelumDibayar)
            .SumAsync(f => f.Jumlah);
}
