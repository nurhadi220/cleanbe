using KosKu.Application.Common;
using KosKu.Application.Features.Auth.DTOs;
using KosKu.Application.Interfaces;
using KosKu.Application.Interfaces.Services;
using KosKu.Domain.Entities;
using KosKu.Domain.Enums;

namespace KosKu.Application.Features.Auth.Services;

/// <summary>Service untuk autentikasi pengguna (register dan login).</summary>
public class AuthService
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public AuthService(IUnitOfWork uow, IPasswordHasher passwordHasher, IJwtService jwtService)
    {
        _uow = uow;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    /// <summary>Mendaftarkan pengguna baru ke sistem.</summary>
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var exists = await _uow.Users.ExistsAsync(u => u.Email.ToLower() == request.Email.ToLower());
        if (exists)
            throw new ValidationException("Email sudah terdaftar.");

        var user = new User
        {
            Nama = request.Nama,
            Email = request.Email.ToLower(),
            Password = _passwordHasher.Hash(request.Password),
            NoHp = request.NoHp,
            Role = request.Role
        };

        await _uow.Users.AddAsync(user);
        await _uow.SaveChangesAsync();

        var token = _jwtService.GenerateToken(user);
        return new AuthResponse(user.Id, user.Nama, user.Email, user.Role.ToString(), token);
    }

    /// <summary>Login pengguna dan mendapatkan JWT token.</summary>
    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _uow.Users.GetByEmailAsync(request.Email);
        if (user is null || !_passwordHasher.Verify(request.Password, user.Password))
            throw new ValidationException("Email atau password salah.");

        var token = _jwtService.GenerateToken(user);
        return new AuthResponse(user.Id, user.Nama, user.Email, user.Role.ToString(), token);
    }
}
