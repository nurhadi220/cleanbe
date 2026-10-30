using KosKu.Domain.Entities;
using KosKu.Domain.Enums;

namespace KosKu.Application.Interfaces.Repositories;

/// <summary>Repository khusus untuk entity Booking.</summary>
public interface IBookingRepository : IGenericRepository<Booking>
{
    /// <summary>Mendapatkan booking dengan detail user dan kamar.</summary>
    Task<IEnumerable<Booking>> GetAllWithDetailsAsync();

    /// <summary>Mendapatkan booking berdasarkan Id dengan detail.</summary>
    Task<Booking?> GetByIdWithDetailsAsync(Guid id);

    /// <summary>Mendapatkan booking berdasarkan kode booking.</summary>
    Task<Booking?> GetByKodeAsync(string kode);

    /// <summary>Mendapatkan booking berdasarkan user.</summary>
    Task<IEnumerable<Booking>> GetByUserIdAsync(Guid userId);

    /// <summary>Jumlah booking pending.</summary>
    Task<int> CountPendingAsync();
}
