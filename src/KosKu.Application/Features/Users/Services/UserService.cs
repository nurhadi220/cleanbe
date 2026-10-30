using KosKu.Application.Common;
using KosKu.Application.Features.Users.DTOs;
using KosKu.Application.Interfaces;
using KosKu.Application.Interfaces.Services;

namespace KosKu.Application.Features.Users.Services;

/// <summary>Service untuk manajemen data pengguna.</summary>
public class UserService
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUnitOfWork uow, IPasswordHasher passwordHasher)
    {
        _uow = uow;
        _passwordHasher = passwordHasher;
    }

    /// <summary>Mendapatkan semua pengguna.</summary>
    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _uow.Users.GetAllAsync();
        return users.Select(u => new UserDto(u.Id, u.Nama, u.Email, u.NoHp, u.Role.ToString(), u.CreatedAt));
    }

    /// <summary>Mendapatkan pengguna berdasarkan Id.</summary>
    public async Task<UserDto> GetByIdAsync(Guid id)
    {
        var user = await _uow.Users.GetByIdAsync(id)
            ?? throw new NotFoundException("User", id);
        return new UserDto(user.Id, user.Nama, user.Email, user.NoHp, user.Role.ToString(), user.CreatedAt);
    }

    /// <summary>Memperbarui data pengguna.</summary>
    public async Task<UserDto> UpdateAsync(Guid id, UpdateUserRequest request)
    {
        var user = await _uow.Users.GetByIdAsync(id)
            ?? throw new NotFoundException("User", id);

        user.Nama = request.Nama;
        user.NoHp = request.NoHp;

        _uow.Users.Update(user);
        await _uow.SaveChangesAsync();

        return new UserDto(user.Id, user.Nama, user.Email, user.NoHp, user.Role.ToString(), user.CreatedAt);
    }

    /// <summary>Menghapus pengguna dari sistem.</summary>
    public async Task DeleteAsync(Guid id)
    {
        var user = await _uow.Users.GetByIdAsync(id)
            ?? throw new NotFoundException("User", id);
        _uow.Users.Delete(user);
        await _uow.SaveChangesAsync();
    }

    /// <summary>Mengganti password pengguna.</summary>
    public async Task ChangePasswordAsync(Guid id, ChangePasswordRequest request)
    {
        var user = await _uow.Users.GetByIdAsync(id)
            ?? throw new NotFoundException("User", id);

        if (!_passwordHasher.Verify(request.PasswordLama, user.Password))
            throw new ValidationException("Password lama tidak sesuai.");

        user.Password = _passwordHasher.Hash(request.PasswordBaru);
        _uow.Users.Update(user);
        await _uow.SaveChangesAsync();
    }
}
