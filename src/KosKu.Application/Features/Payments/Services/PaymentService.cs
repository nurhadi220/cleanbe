using KosKu.Application.Common;
using KosKu.Application.Features.Payments.DTOs;
using KosKu.Application.Interfaces;
using KosKu.Domain.Entities;
using KosKu.Domain.Enums;

namespace KosKu.Application.Features.Payments.Services;

/// <summary>Service untuk manajemen pembayaran sewa.</summary>
public class PaymentService
{
    private readonly IUnitOfWork _uow;

    public PaymentService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<PaymentDto>> GetAllAsync()
    {
        var payments = await _uow.Payments.GetAllWithDetailsAsync();
        return payments.Select(MapToDto);
    }

    public async Task<IEnumerable<PaymentDto>> GetByRentalAsync(Guid rentalId)
    {
        var payments = await _uow.Payments.GetByRentalIdAsync(rentalId);
        return payments.Select(MapToDto);
    }

    public async Task<IEnumerable<PaymentDto>> GetByUserAsync(Guid userId)
    {
        var payments = await _uow.Payments.GetByUserIdAsync(userId);
        return payments.Select(MapToDto);
    }

    /// <summary>Membuat pembayaran baru untuk sewa.</summary>
    public async Task<PaymentDto> CreateAsync(Guid userId, CreatePaymentRequest request)
    {
        var rental = await _uow.Rentals.GetByIdWithDetailsAsync(request.SewaId)
            ?? throw new NotFoundException("Sewa", request.SewaId);

        if (rental.Status != RentalStatus.Aktif && rental.Status != RentalStatus.Diperpanjang)
            throw new ValidationException("Pembayaran hanya dapat dibuat untuk sewa yang aktif.");

        var payment = new Payment
        {
            SewaId = request.SewaId,
            UserId = userId,
            Jumlah = request.Jumlah,
            TanggalBayar = DateTime.UtcNow,
            MetodePembayaran = request.MetodePembayaran,
            StatusPembayaran = PaymentStatus.Lunas,
            BuktiPembayaran = request.BuktiPembayaran
        };

        await _uow.Payments.AddAsync(payment);

        // Update jatuh tempo berikutnya
        var updatedRental = await _uow.Rentals.GetByIdAsync(request.SewaId);
        if (updatedRental is not null)
        {
            updatedRental.JatuhTempoBerikutnya = updatedRental.JatuhTempoBerikutnya.AddMonths(1);
            _uow.Rentals.Update(updatedRental);
        }

        await _uow.SaveChangesAsync();

        var p = await _uow.Payments.GetByIdAsync(payment.Id);
        return MapToDto(payment);
    }

    private static PaymentDto MapToDto(Payment p) =>
        new(p.Id, p.SewaId, p.UserId, p.User?.Nama ?? "", p.Sewa?.Kamar?.NomorKamar ?? "",
            p.Jumlah, p.TanggalBayar, p.MetodePembayaran, p.StatusPembayaran.ToString(),
            p.BuktiPembayaran, p.CreatedAt);
}
