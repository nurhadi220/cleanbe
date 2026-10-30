using FluentValidation;
using KosKu.Application.Features.Bookings.DTOs;

namespace KosKu.Application.Features.Bookings.Validators;

public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(x => x.KamarId)
            .NotEmpty().WithMessage("Kamar wajib dipilih.");

        RuleFor(x => x.TanggalMasuk)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Tanggal masuk tidak boleh di masa lalu.");

        RuleFor(x => x.DurasiBulan)
            .InclusiveBetween(1, 24)
            .WithMessage("Durasi sewa antara 1 hingga 24 bulan.");
    }
}
