using KosKu.Domain.Entities;

namespace KosKu.Application.Interfaces.Repositories;

/// <summary>Repository khusus untuk entity Payment (pembayaran).</summary>
public interface IPaymentRepository : IGenericRepository<Payment>
{
    /// <summary>Mendapatkan pembayaran dengan detail sewa dan user.</summary>
    Task<IEnumerable<Payment>> GetAllWithDetailsAsync();

    /// <summary>Mendapatkan pembayaran berdasarkan sewa.</summary>
    Task<IEnumerable<Payment>> GetByRentalIdAsync(Guid sewaId);

    /// <summary>Mendapatkan pembayaran berdasarkan user.</summary>
    Task<IEnumerable<Payment>> GetByUserIdAsync(Guid userId);

    /// <summary>Total pendapatan pada bulan dan tahun tertentu.</summary>
    Task<decimal> GetMonthlyIncomeAsync(int year, int month);
}
