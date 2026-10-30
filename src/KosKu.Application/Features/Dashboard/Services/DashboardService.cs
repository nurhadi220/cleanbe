using KosKu.Application.Features.Dashboard.DTOs;
using KosKu.Application.Interfaces;

namespace KosKu.Application.Features.Dashboard.Services;

/// <summary>Service untuk data ringkasan dashboard admin.</summary>
public class DashboardService
{
    private readonly IUnitOfWork _uow;

    public DashboardService(IUnitOfWork uow) => _uow = uow;

    /// <summary>Mendapatkan semua data statistik untuk dashboard.</summary>
    public async Task<DashboardDto> GetSummaryAsync()
    {
        var now = DateTime.UtcNow;

        var totalKamar = await _uow.Rooms.CountTotalAsync();
        var penghuniAktif = await _uow.Rentals.CountActiveAsync();
        var bookingPending = await _uow.Bookings.CountPendingAsync();
        var pendapatan = await _uow.Payments.GetMonthlyIncomeAsync(now.Year, now.Month);
        var totalDenda = await _uow.Fines.GetTotalUnpaidAsync();

        return new DashboardDto(totalKamar, penghuniAktif, bookingPending, pendapatan, totalDenda);
    }
}
