namespace KosKu.Application.Interfaces.Services;

/// <summary>Interface untuk layanan hashing password.</summary>
public interface IPasswordHasher
{
    /// <summary>Hash password menggunakan BCrypt.</summary>
    string Hash(string password);

    /// <summary>Verifikasi password terhadap hash.</summary>
    bool Verify(string password, string hash);
}
