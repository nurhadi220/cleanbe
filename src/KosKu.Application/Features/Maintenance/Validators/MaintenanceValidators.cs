using FluentValidation;
using KosKu.Application.Features.Maintenance.DTOs;

namespace KosKu.Application.Features.Maintenance.Validators;

public class CreateMaintenanceRequestValidator : AbstractValidator<CreateMaintenanceRequest>
{
    public CreateMaintenanceRequestValidator()
    {
        RuleFor(x => x.KamarId)
            .NotEmpty().WithMessage("Kamar wajib dipilih.");

        RuleFor(x => x.JudulLaporan)
            .NotEmpty().WithMessage("Judul laporan wajib diisi.")
            .MaximumLength(200).WithMessage("Judul laporan maksimal 200 karakter.");

        RuleFor(x => x.Deskripsi)
            .NotEmpty().WithMessage("Deskripsi wajib diisi.");
    }
}
