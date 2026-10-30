using KosKu.Domain.Entities;

namespace KosKu.Application.Interfaces.Repositories;

/// <summary>Repository khusus untuk entity Fine (denda).</summary>
public interface IFineRepository : IGenericRepository<Fine>
{
    /// <summary>Mendapatkan denda berdasarkan sewa.</summary>
    Task<IEnumerable<Fine>> GetByRentalIdAsync(Guid sewaId);

    /// <summary>Total jumlah denda yang belum dibayar.</summary>
    Task<decimal> GetTotalUnpaidAsync();
}
