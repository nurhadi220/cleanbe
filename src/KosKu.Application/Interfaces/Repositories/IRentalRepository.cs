using KosKu.Domain.Entities;

namespace KosKu.Application.Interfaces.Repositories;

/// <summary>Repository khusus untuk entity Rental (sewa aktif).</summary>
public interface IRentalRepository : IGenericRepository<Rental>
{
    /// <summary>Mendapatkan sewa dengan detail booking, user, dan kamar.</summary>
    Task<IEnumerable<Rental>> GetAllWithDetailsAsync();

    /// <summary>Mendapatkan sewa berdasarkan Id dengan detail.</summary>
    Task<Rental?> GetByIdWithDetailsAsync(Guid id);

    /// <summary>Mendapatkan sewa berdasarkan user.</summary>
    Task<IEnumerable<Rental>> GetByUserIdAsync(Guid userId);

    /// <summary>Mendapatkan sewa aktif berdasarkan kamar.</summary>
    Task<Rental?> GetActiveByRoomIdAsync(Guid kamarId);

    /// <summary>Jumlah penghuni aktif.</summary>
    Task<int> CountActiveAsync();
}
