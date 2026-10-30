using KosKu.Application.Interfaces.Repositories;
using KosKu.Domain.Entities;
using KosKu.Domain.Enums;
using KosKu.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KosKu.Infrastructure.Repositories;

/// <summary>Implementasi repository untuk Payment (pembayaran).</summary>
public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(AppDbContext context) : base(context) { }

    /// <inheritdoc/>
    public async Task<IEnumerable<Payment>> GetAllWithDetailsAsync()
        => await _dbSet.AsNoTracking()
            .Include(p => p.User)
            .Include(p => p.Sewa).ThenInclude(s => s.Kamar)
            .OrderByDescending(p => p.TanggalBayar)
            .ToListAsync();

    /// <inheritdoc/>
    public async Task<IEnumerable<Payment>> GetByRentalIdAsync(Guid sewaId)
        => await _dbSet.AsNoTracking()
            .Where(p => p.SewaId == sewaId)
            .OrderByDescending(p => p.TanggalBayar)
            .ToListAsync();

    /// <inheritdoc/>
    public async Task<IEnumerable<Payment>> GetByUserIdAsync(Guid userId)
        => await _dbSet.AsNoTracking()
            .Include(p => p.Sewa).ThenInclude(s => s.Kamar)
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.TanggalBayar)
            .ToListAsync();

    /// <inheritdoc/>
    public async Task<decimal> GetMonthlyIncomeAsync(int year, int month)
        => await _dbSet
            .Where(p => p.StatusPembayaran == PaymentStatus.Lunas
                     && p.TanggalBayar.Year == year
                     && p.TanggalBayar.Month == month)
            .SumAsync(p => p.Jumlah);
}
