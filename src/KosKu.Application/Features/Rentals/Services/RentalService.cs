using KosKu.Application.Common;
using KosKu.Application.Features.Rentals.DTOs;
using KosKu.Application.Interfaces;
using KosKu.Domain.Enums;

namespace KosKu.Application.Features.Rentals.Services;

/// <summary>Service untuk manajemen sewa aktif.</summary>
public class RentalService
{
    private readonly IUnitOfWork _uow;

    public RentalService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<RentalDto>> GetAllAsync()
    {
        var rentals = await _uow.Rentals.GetAllWithDetailsAsync();
        return rentals.Select(MapToDto);
    }

    public async Task<RentalDto> GetByIdAsync(Guid id)
    {
        var rental = await _uow.Rentals.GetByIdWithDetailsAsync(id)
            ?? throw new NotFoundException("Sewa", id);
        return MapToDto(rental);
    }

    public async Task<IEnumerable<RentalDto>> GetByUserAsync(Guid userId)
    {
        var rentals = await _uow.Rentals.GetByUserIdAsync(userId);
        return rentals.Select(MapToDto);
    }

    /// <summary>Menyelesaikan sewa aktif.</summary>
    public async Task<RentalDto> FinishAsync(Guid rentalId)
    {
        var rental = await _uow.Rentals.GetByIdAsync(rentalId)
            ?? throw new NotFoundException("Sewa", rentalId);

        if (rental.Status != RentalStatus.Aktif)
            throw new ValidationException("Hanya sewa aktif yang dapat diselesaikan.");

        rental.Status = RentalStatus.Selesai;
        _uow.Rentals.Update(rental);

        var room = await _uow.Rooms.GetByIdAsync(rental.KamarId);
        if (room is not null)
        {
            room.Status = RoomStatus.Tersedia;
            _uow.Rooms.Update(room);
        }

        await _uow.SaveChangesAsync();
        var updated = await _uow.Rentals.GetByIdWithDetailsAsync(rentalId);
        return MapToDto(updated!);
    }

    /// <summary>Memperpanjang masa sewa.</summary>
    public async Task<RentalDto> ExtendAsync(Guid rentalId, ExtendRentalRequest request)
    {
        var rental = await _uow.Rentals.GetByIdAsync(rentalId)
            ?? throw new NotFoundException("Sewa", rentalId);

        if (rental.Status != RentalStatus.Aktif)
            throw new ValidationException("Hanya sewa aktif yang dapat diperpanjang.");

        rental.TanggalSelesai = rental.TanggalSelesai.AddMonths(request.TambahanBulan);
        rental.JatuhTempoBerikutnya = rental.JatuhTempoBerikutnya.AddMonths(request.TambahanBulan);
        rental.Status = RentalStatus.Diperpanjang;
        _uow.Rentals.Update(rental);
        await _uow.SaveChangesAsync();

        var updated = await _uow.Rentals.GetByIdWithDetailsAsync(rentalId);
        return MapToDto(updated!);
    }

    private static RentalDto MapToDto(Domain.Entities.Rental r) =>
        new(r.Id, r.BookingId, r.Booking?.KodeBooking ?? "", r.UserId, r.User?.Nama ?? "",
            r.KamarId, r.Kamar?.NomorKamar ?? "", r.Kamar?.TipeKamar?.NamaTipe ?? "",
            r.Kamar?.TipeKamar?.HargaPerBulan ?? 0,
            r.TanggalMulai, r.TanggalSelesai, r.JatuhTempoBerikutnya,
            r.Status.ToString(), r.CreatedAt);
}
