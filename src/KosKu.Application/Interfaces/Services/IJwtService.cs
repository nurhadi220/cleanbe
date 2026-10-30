using KosKu.Domain.Entities;

namespace KosKu.Application.Interfaces.Services;

/// <summary>Interface untuk layanan JWT token.</summary>
public interface IJwtService
{
    /// <summary>Menghasilkan JWT token untuk user yang terautentikasi.</summary>
    /// <param name="user">Entity user yang telah terverifikasi.</param>
    /// <returns>JWT token string.</returns>
    string GenerateToken(User user);
}
