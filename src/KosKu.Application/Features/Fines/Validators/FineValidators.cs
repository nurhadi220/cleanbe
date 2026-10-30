using FluentValidation;
using KosKu.Application.Features.Fines.DTOs;

namespace KosKu.Application.Features.Fines.Validators;

public class CreateFineRequestValidator : AbstractValidator<CreateFineRequest>
{
    public CreateFineRequestValidator()
    {
        RuleFor(x => x.SewaId)
            .NotEmpty().WithMessage("Sewa ID wajib diisi.");

        RuleFor(x => x.Keterangan)
            .NotEmpty().WithMessage("Keterangan denda wajib diisi.")
            .MaximumLength(500).WithMessage("Keterangan maksimal 500 karakter.");

        RuleFor(x => x.Jumlah)
            .GreaterThan(0).WithMessage("Jumlah denda harus lebih dari 0.");
    }
}
