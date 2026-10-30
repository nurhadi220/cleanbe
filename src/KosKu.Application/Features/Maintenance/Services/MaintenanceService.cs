using KosKu.Application.Common;
using KosKu.Application.Features.Maintenance.DTOs;
using KosKu.Application.Interfaces;
using KosKu.Domain.Entities;
using KosKu.Domain.Enums;

namespace KosKu.Application.Features.Maintenance.Services;

/// <summary>Service untuk manajemen laporan maintenance/perbaikan.</summary>
public class MaintenanceService
{
    private readonly IUnitOfWork _uow;

    public MaintenanceService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<MaintenanceDto>> GetAllAsync()
    {
        var reports = await _uow.Maintenance.GetAllWithDetailsAsync();
        return reports.Select(MapToDto);
    }

    public async Task<IEnumerable<MaintenanceDto>> GetByRoomAsync(Guid kamarId)
    {
        var reports = await _uow.Maintenance.GetByRoomIdAsync(kamarId);
        return reports.Select(MapToDto);
    }

    public async Task<IEnumerable<MaintenanceDto>> GetByUserAsync(Guid userId)
    {
        var reports = await _uow.Maintenance.GetByUserIdAsync(userId);
        return reports.Select(MapToDto);
    }

    /// <summary>Penghuni membuat laporan kerusakan/perbaikan.</summary>
    public async Task<MaintenanceDto> CreateAsync(Guid userId, CreateMaintenanceRequest request)
    {
        var room = await _uow.Rooms.GetByIdAsync(request.KamarId)
            ?? throw new NotFoundException("Kamar", request.KamarId);

        var report = new MaintenanceRequest
        {
            KamarId = request.KamarId,
            UserId = userId,
            JudulLaporan = request.JudulLaporan,
            Deskripsi = request.Deskripsi,
            Status = MaintenanceStatus.Pending
        };

        await _uow.Maintenance.AddAsync(report);
        await _uow.SaveChangesAsync();

        var created = await _uow.Maintenance.GetAllWithDetailsAsync();
        var result = created.FirstOrDefault(m => m.Id == report.Id);
        return MapToDto(result ?? report);
    }

    /// <summary>Admin memperbarui status laporan.</summary>
    public async Task<MaintenanceDto> UpdateStatusAsync(Guid reportId, UpdateMaintenanceStatusRequest request)
    {
        var report = await _uow.Maintenance.GetByIdAsync(reportId)
            ?? throw new NotFoundException("LaporanPerbaikan", reportId);

        report.Status = request.Status;
        _uow.Maintenance.Update(report);
        await _uow.SaveChangesAsync();

        var updated = await _uow.Maintenance.GetAllWithDetailsAsync();
        var result = updated.FirstOrDefault(m => m.Id == reportId);
        return MapToDto(result ?? report);
    }

    private static MaintenanceDto MapToDto(MaintenanceRequest m) =>
        new(m.Id, m.KamarId, m.Kamar?.NomorKamar ?? "", m.UserId, m.User?.Nama ?? "",
            m.JudulLaporan, m.Deskripsi, m.Status.ToString(), m.CreatedAt);
}
