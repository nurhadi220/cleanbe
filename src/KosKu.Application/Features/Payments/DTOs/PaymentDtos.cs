namespace KosKu.Application.Features.Payments.DTOs;

public record PaymentDto(
    Guid Id,
    Guid SewaId,
    Guid UserId,
    string NamaPenghuni,
    string NomorKamar,
    decimal Jumlah,
    DateTime TanggalBayar,
    string MetodePembayaran,
    string StatusPembayaran,
    string? BuktiPembayaran,
    DateTime CreatedAt
);

public record CreatePaymentRequest(
    Guid SewaId,
    decimal Jumlah,
    string MetodePembayaran,
    string? BuktiPembayaran
);
