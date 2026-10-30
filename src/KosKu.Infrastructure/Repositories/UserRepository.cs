using KosKu.Application.Interfaces.Repositories;
using KosKu.Domain.Entities;
using KosKu.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KosKu.Infrastructure.Repositories;

/// <summary>Implementasi repository untuk User.</summary>
public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    /// <inheritdoc/>
    public async Task<User?> GetByEmailAsync(string email)
        => await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
}
