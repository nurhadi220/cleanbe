using KosKu.Domain.Enums;

namespace KosKu.Application.Features.Maintenance.DTOs;

public record MaintenanceDto(
    Guid Id,
    Guid KamarId,
    string NomorKamar,
    Guid UserId,
    string NamaPelapor,
    string JudulLaporan,
    string Deskripsi,
    string Status,
    DateTime CreatedAt
);

public record CreateMaintenanceRequest(
    Guid KamarId,
    string JudulLaporan,
    string Deskripsi
);

public record UpdateMaintenanceStatusRequest(
    MaintenanceStatus Status
);
