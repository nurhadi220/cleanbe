using KosKu.Domain.Entities;

namespace KosKu.Application.Interfaces.Repositories;

/// <summary>Repository khusus untuk entity RoomType.</summary>
public interface IRoomTypeRepository : IGenericRepository<RoomType>
{
    /// <summary>Mendapatkan tipe kamar beserta daftar kamarnya.</summary>
    Task<RoomType?> GetWithRoomsAsync(Guid id);
}
