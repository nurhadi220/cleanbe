using KosKu.Domain.Enums;

namespace KosKu.Application.Features.Bookings.DTOs;

public record BookingDto(
    Guid Id,
    string KodeBooking,
    Guid UserId,
    string NamaPenghuni,
    Guid KamarId,
    string NomorKamar,
    string NamaTipe,
    DateOnly TanggalMasuk,
    int DurasiBulan,
    string Status,
    DateTime CreatedAt
);

public record CreateBookingRequest(
    Guid KamarId,
    DateOnly TanggalMasuk,
    int DurasiBulan
);

public record ApproveBookingRequest(
    Guid BookingId
);
