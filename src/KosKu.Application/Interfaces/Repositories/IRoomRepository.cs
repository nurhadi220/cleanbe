using KosKu.Domain.Entities;
using KosKu.Domain.Enums;

namespace KosKu.Application.Interfaces.Repositories;

/// <summary>Repository khusus untuk entity Room.</summary>
public interface IRoomRepository : IGenericRepository<Room>
{
    /// <summary>Mendapatkan kamar dengan informasi tipe kamar.</summary>
    Task<IEnumerable<Room>> GetAllWithTypeAsync();

    /// <summary>Mendapatkan kamar berdasarkan Id dengan informasi tipe kamar.</summary>
    Task<Room?> GetByIdWithTypeAsync(Guid id);

    /// <summary>Mendapatkan kamar berdasarkan status.</summary>
    Task<IEnumerable<Room>> GetByStatusAsync(RoomStatus status);

    /// <summary>Jumlah total kamar.</summary>
    Task<int> CountTotalAsync();
}
