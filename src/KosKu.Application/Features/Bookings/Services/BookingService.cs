using KosKu.Application.Common;
using KosKu.Application.Features.Bookings.DTOs;
using KosKu.Application.Interfaces;
using KosKu.Domain.Entities;
using KosKu.Domain.Enums;

namespace KosKu.Application.Features.Bookings.Services;

/// <summary>Service untuk manajemen booking kamar.</summary>
public class BookingService
{
    private readonly IUnitOfWork _uow;

    public BookingService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<BookingDto>> GetAllAsync()
    {
        var bookings = await _uow.Bookings.GetAllWithDetailsAsync();
        return bookings.Select(MapToDto);
    }

    public async Task<BookingDto> GetByIdAsync(Guid id)
    {
        var booking = await _uow.Bookings.GetByIdWithDetailsAsync(id)
            ?? throw new NotFoundException("Booking", id);
        return MapToDto(booking);
    }

    public async Task<IEnumerable<BookingDto>> GetByUserAsync(Guid userId)
    {
        var bookings = await _uow.Bookings.GetByUserIdAsync(userId);
        return bookings.Select(MapToDto);
    }

    /// <summary>Membuat booking baru oleh penghuni.</summary>
    public async Task<BookingDto> CreateAsync(Guid userId, CreateBookingRequest request)
    {
        var room = await _uow.Rooms.GetByIdWithTypeAsync(request.KamarId)
            ?? throw new NotFoundException("Kamar", request.KamarId);

        if (room.Status != RoomStatus.Tersedia)
            throw new ValidationException("Kamar tidak tersedia untuk di-booking.");

        // Buat kode booking unik
        var kode = $"BK{DateTime.UtcNow:yyyyMMddHHmmss}{new Random().Next(100, 999)}";

        var booking = new Booking
        {
            KodeBooking = kode,
            UserId = userId,
            KamarId = request.KamarId,
            TanggalMasuk = request.TanggalMasuk,
            DurasiBuilan = request.DurasiBulan,
            Status = BookingStatus.Pending
        };

        await _uow.Bookings.AddAsync(booking);
        await _uow.SaveChangesAsync();

        var booked = await _uow.Bookings.GetByIdWithDetailsAsync(booking.Id);
        return MapToDto(booked!);
    }

    /// <summary>Admin mengonfirmasi booking dan otomatis membuat sewa aktif.</summary>
    public async Task<BookingDto> ApproveAsync(Guid bookingId)
    {
        var booking = await _uow.Bookings.GetByIdAsync(bookingId)
            ?? throw new NotFoundException("Booking", bookingId);

        if (booking.Status != BookingStatus.Pending)
            throw new ValidationException("Hanya booking dengan status Pending yang dapat dikonfirmasi.");

        booking.Status = BookingStatus.Dikonfirmasi;
        _uow.Bookings.Update(booking);

        // Update status kamar jadi Disewa
        var room = await _uow.Rooms.GetByIdAsync(booking.KamarId)!
            ?? throw new NotFoundException("Kamar", booking.KamarId);
        room.Status = RoomStatus.Disewa;
        _uow.Rooms.Update(room);

        // Buat sewa aktif otomatis
        var tanggalMulai = booking.TanggalMasuk;
        var tanggalSelesai = tanggalMulai.AddMonths(booking.DurasiBuilan);
        var rental = new Rental
        {
            BookingId = booking.Id,
            UserId = booking.UserId,
            KamarId = booking.KamarId,
            TanggalMulai = tanggalMulai,
            TanggalSelesai = tanggalSelesai,
            JatuhTempoBerikutnya = tanggalMulai.AddMonths(1),
            Status = RentalStatus.Aktif
        };

        await _uow.Rentals.AddAsync(rental);
        await _uow.SaveChangesAsync();

        var updated = await _uow.Bookings.GetByIdWithDetailsAsync(bookingId);
        return MapToDto(updated!);
    }

    /// <summary>Membatalkan booking.</summary>
    public async Task<BookingDto> CancelAsync(Guid bookingId)
    {
        var booking = await _uow.Bookings.GetByIdWithDetailsAsync(bookingId)
            ?? throw new NotFoundException("Booking", bookingId);

        if (booking.Status == BookingStatus.Dibatalkan)
            throw new ValidationException("Booking sudah dibatalkan sebelumnya.");
        if (booking.Status == BookingStatus.Dikonfirmasi)
            throw new ValidationException("Booking yang sudah dikonfirmasi tidak dapat dibatalkan langsung.");

        booking.Status = BookingStatus.Dibatalkan;
        _uow.Bookings.Update(booking);
        await _uow.SaveChangesAsync();

        return MapToDto(booking);
    }

    private static BookingDto MapToDto(Booking b) =>
        new(b.Id, b.KodeBooking, b.UserId, b.User?.Nama ?? "",
            b.KamarId, b.Kamar?.NomorKamar ?? "", b.Kamar?.TipeKamar?.NamaTipe ?? "",
            b.TanggalMasuk, b.DurasiBuilan, b.Status.ToString(), b.CreatedAt);
}
