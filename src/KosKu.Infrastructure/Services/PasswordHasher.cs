using KosKu.Application.Interfaces.Services;

namespace KosKu.Infrastructure.Services;

/// <summary>Implementasi password hashing menggunakan BCrypt.</summary>
public class PasswordHasher : IPasswordHasher
{
    /// <inheritdoc/>
    public string Hash(string password)
        => BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));

    /// <inheritdoc/>
    public bool Verify(string password, string hash)
        => BCrypt.Net.BCrypt.Verify(password, hash);
}
