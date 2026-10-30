using KosKu.Infrastructure.Services;
using Xunit;

namespace KosKu.UnitTests.Services;

public class PasswordHasherTests
{
    private readonly PasswordHasher _hasher;

    public PasswordHasherTests()
    {
        _hasher = new PasswordHasher();
    }

    [Fact]
    public void Hash_ShouldReturnHashedPassword()
    {
        // Arrange
        var password = "SuperSecretPassword123#";

        // Act
        var hash = _hasher.Hash(password);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        Assert.NotEqual(password, hash);
    }

    [Fact]
    public void Verify_WithCorrectPassword_ShouldReturnTrue()
    {
        // Arrange
        var password = "SuperSecretPassword123#";
        var hash = _hasher.Hash(password);

        // Act
        var result = _hasher.Verify(password, hash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Verify_WithIncorrectPassword_ShouldReturnFalse()
    {
        // Arrange
        var password = "SuperSecretPassword123#";
        var wrongPassword = "WrongPassword123#";
        var hash = _hasher.Hash(password);

        // Act
        var result = _hasher.Verify(wrongPassword, hash);

        // Assert
        Assert.False(result);
    }
}
