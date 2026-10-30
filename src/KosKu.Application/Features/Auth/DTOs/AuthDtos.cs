using KosKu.Domain.Enums;

namespace KosKu.Application.Features.Auth.DTOs;

public record RegisterRequest(
    string Nama,
    string Email,
    string Password,
    string? NoHp,
    UserRole Role = UserRole.Penghuni
);

public record LoginRequest(
    string Email,
    string Password
);

public record AuthResponse(
    Guid UserId,
    string Nama,
    string Email,
    string Role,
    string Token
);
