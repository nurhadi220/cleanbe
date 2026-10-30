using KosKu.Domain.Entities;

namespace KosKu.Application.Interfaces.Repositories;

/// <summary>Repository khusus untuk entity User.</summary>
public interface IUserRepository : IGenericRepository<User>
{
    /// <summary>Mendapatkan user berdasarkan email.</summary>
    Task<User?> GetByEmailAsync(string email);
}
