using System.Collections.Generic;

namespace KosKu.Application.Features.RoomTypes.DTOs;

public record RoomTypeDto(
    Guid Id,
    string NamaTipe,
    string? Deskripsi,
    decimal HargaPerBulan,
    FasilitasDto? Fasilitas,
    DateTime CreatedAt
);

public record CreateRoomTypeRequest(
    string NamaTipe,
    string? Deskripsi,
    decimal HargaPerBulan,
    FasilitasDto? Fasilitas
);

public record UpdateRoomTypeRequest(
    string NamaTipe,
    string? Deskripsi,
    decimal HargaPerBulan,
    FasilitasDto? Fasilitas
);

