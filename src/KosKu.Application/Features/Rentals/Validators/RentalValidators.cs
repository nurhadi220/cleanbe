using FluentValidation;
using KosKu.Application.Features.Rentals.DTOs;

namespace KosKu.Application.Features.Rentals.Validators;

public class ExtendRentalRequestValidator : AbstractValidator<ExtendRentalRequest>
{
    public ExtendRentalRequestValidator()
    {
        RuleFor(x => x.TambahanBulan)
            .InclusiveBetween(1, 12)
            .WithMessage("Perpanjangan antara 1 hingga 12 bulan.");
    }
}
