using KosKu.Application.Common;
using KosKu.Application.Features.Fines.DTOs;
using KosKu.Application.Interfaces;
using KosKu.Domain.Entities;
using KosKu.Domain.Enums;

namespace KosKu.Application.Features.Fines.Services;

/// <summary>Service untuk manajemen denda.</summary>
public class FineService
{
    private readonly IUnitOfWork _uow;

    public FineService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<FineDto>> GetAllAsync()
    {
        var fines = await _uow.Fines.GetAllAsync();
        return await MapToDtosAsync(fines);
    }

    public async Task<IEnumerable<FineDto>> GetByRentalAsync(Guid rentalId)
    {
        var fines = await _uow.Fines.GetByRentalIdAsync(rentalId);
        return await MapToDtosAsync(fines);
    }

    /// <summary>Admin membuat denda untuk sewa.</summary>
    public async Task<FineDto> CreateAsync(CreateFineRequest request)
    {
        var rental = await _uow.Rentals.GetByIdWithDetailsAsync(request.SewaId)
            ?? throw new NotFoundException("Sewa", request.SewaId);

        var fine = new Fine
        {
            SewaId = request.SewaId,
            Keterangan = request.Keterangan,
            Jumlah = request.Jumlah,
            Status = FineStatus.BelumDibayar
        };

        await _uow.Fines.AddAsync(fine);
        await _uow.SaveChangesAsync();

        return new FineDto(fine.Id, fine.SewaId, rental.Kamar?.NomorKamar ?? "",
            fine.Keterangan, fine.Jumlah, fine.Status.ToString(), fine.CreatedAt);
    }

    /// <summary>Melunasi denda.</summary>
    public async Task<FineDto> PayAsync(Guid fineId)
    {
        var fine = await _uow.Fines.GetByIdAsync(fineId)
            ?? throw new NotFoundException("Denda", fineId);

        if (fine.Status == FineStatus.Lunas)
            throw new ValidationException("Denda ini sudah lunas.");

        fine.Status = FineStatus.Lunas;
        _uow.Fines.Update(fine);
        await _uow.SaveChangesAsync();

        var rental = await _uow.Rentals.GetByIdWithDetailsAsync(fine.SewaId);
        return new FineDto(fine.Id, fine.SewaId, rental?.Kamar?.NomorKamar ?? "",
            fine.Keterangan, fine.Jumlah, fine.Status.ToString(), fine.CreatedAt);
    }

    private async Task<IEnumerable<FineDto>> MapToDtosAsync(IEnumerable<Fine> fines)
    {
        var result = new List<FineDto>();
        foreach (var f in fines)
        {
            var rental = await _uow.Rentals.GetByIdWithDetailsAsync(f.SewaId);
            result.Add(new FineDto(f.Id, f.SewaId, rental?.Kamar?.NomorKamar ?? "",
                f.Keterangan, f.Jumlah, f.Status.ToString(), f.CreatedAt));
        }
        return result;
    }
}
