using KosKu.Application.Interfaces.Repositories;
using KosKu.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KosKu.Infrastructure.Repositories;

/// <summary>Implementasi generic repository menggunakan EF Core.</summary>
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.AsNoTracking().ToListAsync();

    /// <inheritdoc/>
    public async Task<T?> GetByIdAsync(Guid id)
        => await _dbSet.FindAsync(id);

    /// <inheritdoc/>
    public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);

    /// <inheritdoc/>
    public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.AsNoTracking().Where(predicate).ToListAsync();

    /// <inheritdoc/>
    public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    /// <inheritdoc/>
    public void Update(T entity)
        => _dbSet.Update(entity);

    /// <inheritdoc/>
    public void Delete(T entity)
        => _dbSet.Remove(entity);

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.AnyAsync(predicate);
}
