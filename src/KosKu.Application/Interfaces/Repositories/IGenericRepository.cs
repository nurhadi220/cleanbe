using System.Linq.Expressions;

namespace KosKu.Application.Interfaces.Repositories;

/// <summary>Generic repository interface untuk operasi CRUD dasar.</summary>
/// <typeparam name="T">Tipe entity.</typeparam>
public interface IGenericRepository<T> where T : class
{
    /// <summary>Mendapatkan semua entity.</summary>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>Mendapatkan entity berdasarkan Id.</summary>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>Mendapatkan entity berdasarkan kondisi tertentu.</summary>
    Task<T?> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>Mendapatkan daftar entity berdasarkan kondisi.</summary>
    Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);

    /// <summary>Menambahkan entity baru.</summary>
    Task AddAsync(T entity);

    /// <summary>Memperbarui entity yang ada.</summary>
    void Update(T entity);

    /// <summary>Menghapus entity.</summary>
    void Delete(T entity);

    /// <summary>Mengecek apakah entity dengan kondisi tertentu ada.</summary>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}
