using FluentValidation;
using KosKu.Application.Features.Rooms.DTOs;

namespace KosKu.Application.Features.Rooms.Validators;

public class CreateRoomRequestValidator : AbstractValidator<CreateRoomRequest>
{
    public CreateRoomRequestValidator()
    {
        RuleFor(x => x.NomorKamar)
            .NotEmpty().WithMessage("Nomor kamar wajib diisi.")
            .MaximumLength(20).WithMessage("Nomor kamar maksimal 20 karakter.");

        RuleFor(x => x.Lantai)
            .GreaterThan(0).WithMessage("Lantai harus lebih dari 0.");

        RuleFor(x => x.TipeKamarId)
            .NotEmpty().WithMessage("Tipe kamar wajib dipilih.");
    }
}

public class UpdateRoomRequestValidator : AbstractValidator<UpdateRoomRequest>
{
    public UpdateRoomRequestValidator()
    {
        RuleFor(x => x.NomorKamar)
            .NotEmpty().WithMessage("Nomor kamar wajib diisi.")
            .MaximumLength(20).WithMessage("Nomor kamar maksimal 20 karakter.");

        RuleFor(x => x.Lantai)
            .GreaterThan(0).WithMessage("Lantai harus lebih dari 0.");

        RuleFor(x => x.TipeKamarId)
            .NotEmpty().WithMessage("Tipe kamar wajib dipilih.");
    }
}
