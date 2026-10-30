using KosKu.Domain.Enums;

namespace KosKu.Application.Features.Rooms.DTOs;

public record RoomDto(
    Guid Id,
    string NomorKamar,
    int Lantai,
    Guid TipeKamarId,
    string NamaTipe,
    decimal HargaPerBulan,
    string Status,
    string? FotoUrl,
    DateTime CreatedAt
);

public record CreateRoomRequest(
    string NomorKamar,
    int Lantai,
    Guid TipeKamarId,
    string? FotoUrl
);

public record UpdateRoomRequest(
    string NomorKamar,
    int Lantai,
    Guid TipeKamarId,
    RoomStatus Status,
    string? FotoUrl
);
