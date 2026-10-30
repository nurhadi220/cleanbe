using System.Linq.Expressions;
using KosKu.Application.Common;
using KosKu.Application.Features.Auth.DTOs;
using KosKu.Application.Features.Auth.Services;
using KosKu.Application.Interfaces;
using KosKu.Application.Interfaces.Repositories;
using KosKu.Application.Interfaces.Services;
using KosKu.Domain.Entities;
using KosKu.Domain.Enums;
using NSubstitute;
using Xunit;

namespace KosKu.UnitTests.Services;

public class AuthServiceTests
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _uow = Substitute.For<IUnitOfWork>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _jwtService = Substitute.For<IJwtService>();
        _authService = new AuthService(_uow, _passwordHasher, _jwtService);
    }

    [Fact]
    public async Task RegisterAsync_EmailAlreadyExists_ShouldThrowValidationException()
    {
        // Arrange
        var request = new RegisterRequest("Test User", "exist@example.com", "Password123#", "08123456789");
        _uow.Users.ExistsAsync(Arg.Any<Expression<Func<User, bool>>>()).Returns(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => _authService.RegisterAsync(request));
        Assert.Equal("Email sudah terdaftar.", exception.Message);
        
        await _uow.Users.DidNotReceive().AddAsync(Arg.Any<User>());
        await _uow.DidNotReceive().SaveChangesAsync();
    }


    [Fact]
    public async Task RegisterAsync_ValidRequest_ShouldCreateUserAndReturnResponse()
    {
        // Arrange
        var request = new RegisterRequest("Test User", "new@example.com", "Password123#", "08123456789");
        _uow.Users.ExistsAsync(Arg.Any<Expression<Func<User, bool>>>()).Returns(false);
        _passwordHasher.Hash(request.Password).Returns("hashed_password");
        _jwtService.GenerateToken(Arg.Any<User>()).Returns("mock_jwt_token");

        // Act
        var response = await _authService.RegisterAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(request.Nama, response.Nama);
        Assert.Equal(request.Email.ToLower(), response.Email);
        Assert.Equal("Penghuni", response.Role);
        Assert.Equal("mock_jwt_token", response.Token);

        await _uow.Users.Received(1).AddAsync(Arg.Is<User>(u =>
            u.Nama == request.Nama &&
            u.Email == request.Email.ToLower() &&
            u.Password == "hashed_password" &&
            u.NoHp == request.NoHp &&
            u.Role == UserRole.Penghuni
        ));
        await _uow.Received(1).SaveChangesAsync();
    }


    [Fact]
    public async Task LoginAsync_UserDoesNotExist_ShouldThrowValidationException()
    {
        // Arrange
        var request = new LoginRequest("nonexist@example.com", "Password123#");
        _uow.Users.GetByEmailAsync(request.Email).Returns((User?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => _authService.LoginAsync(request));
        Assert.Equal("Email atau password salah.", exception.Message);
    }

    [Fact]
    public async Task LoginAsync_IncorrectPassword_ShouldThrowValidationException()
    {
        // Arrange
        var request = new LoginRequest("test@example.com", "Password123#");
        var user = new User { Email = request.Email, Password = "correct_hashed_password" };
        _uow.Users.GetByEmailAsync(request.Email).Returns(user);
        _passwordHasher.Verify(request.Password, user.Password).Returns(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => _authService.LoginAsync(request));
        Assert.Equal("Email atau password salah.", exception.Message);
    }

    [Fact]
    public async Task LoginAsync_CorrectCredentials_ShouldReturnAuthResponse()
    {
        // Arrange
        var request = new LoginRequest("test@example.com", "Password123#");
        var user = new User
        {
            Nama = "Test User",
            Email = request.Email,
            Password = "correct_hashed_password",
            Role = UserRole.Admin
        };
        _uow.Users.GetByEmailAsync(request.Email).Returns(user);
        _passwordHasher.Verify(request.Password, user.Password).Returns(true);
        _jwtService.GenerateToken(user).Returns("mock_jwt_token");

        // Act
        var response = await _authService.LoginAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(user.Nama, response.Nama);
        Assert.Equal(user.Email, response.Email);
        Assert.Equal("Admin", response.Role);
        Assert.Equal("mock_jwt_token", response.Token);
    }
}
