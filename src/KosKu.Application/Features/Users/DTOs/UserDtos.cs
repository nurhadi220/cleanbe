using KosKu.Domain.Enums;

namespace KosKu.Application.Features.Users.DTOs;

public record UserDto(
    Guid Id,
    string Nama,
    string Email,
    string? NoHp,
    string Role,
    DateTime CreatedAt
);

public record UpdateUserRequest(
    string Nama,
    string? NoHp
);

public record ChangePasswordRequest(
    string PasswordLama,
    string PasswordBaru
);
