using FluentValidation;
using KosKu.Application.Features.Payments.DTOs;

namespace KosKu.Application.Features.Payments.Validators;

public class CreatePaymentRequestValidator : AbstractValidator<CreatePaymentRequest>
{
    public CreatePaymentRequestValidator()
    {
        RuleFor(x => x.SewaId)
            .NotEmpty().WithMessage("Sewa ID wajib diisi.");

        RuleFor(x => x.Jumlah)
            .GreaterThan(0).WithMessage("Jumlah pembayaran harus lebih dari 0.");

        RuleFor(x => x.MetodePembayaran)
            .NotEmpty().WithMessage("Metode pembayaran wajib diisi.")
            .MaximumLength(50).WithMessage("Metode pembayaran maksimal 50 karakter.");
    }
}
