namespace KosKu.Application.Features.Fines.DTOs;

public record FineDto(
    Guid Id,
    Guid SewaId,
    string NomorKamar,
    string Keterangan,
    decimal Jumlah,
    string Status,
    DateTime CreatedAt
);

public record CreateFineRequest(
    Guid SewaId,
    string Keterangan,
    decimal Jumlah
);
