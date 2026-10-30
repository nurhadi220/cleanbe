namespace KosKu.Application.Features.Rentals.DTOs;

public record RentalDto(
    Guid Id,
    Guid BookingId,
    string KodeBooking,
    Guid UserId,
    string NamaPenghuni,
    Guid KamarId,
    string NomorKamar,
    string NamaTipe,
    decimal HargaPerBulan,
    DateOnly TanggalMulai,
    DateOnly TanggalSelesai,
    DateOnly JatuhTempoBerikutnya,
    string Status,
    DateTime CreatedAt
);

public record ExtendRentalRequest(
    int TambahanBulan
);
