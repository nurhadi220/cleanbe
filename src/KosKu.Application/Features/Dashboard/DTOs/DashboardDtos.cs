namespace KosKu.Application.Features.Dashboard.DTOs;

/// <summary>DTO untuk data ringkasan dashboard.</summary>
public record DashboardDto(
    int TotalKamar,
    int PenghuniAktif,
    int BookingPending,
    decimal PendapatanBulanIni,
    decimal TotalDendaBelumDibayar
);
