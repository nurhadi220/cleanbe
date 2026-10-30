using FluentValidation;
using KosKu.Application.Features.RoomTypes.DTOs;

namespace KosKu.Application.Features.RoomTypes.Validators;

public class CreateRoomTypeRequestValidator : AbstractValidator<CreateRoomTypeRequest>
{
    public CreateRoomTypeRequestValidator()
    {
        RuleFor(x => x.NamaTipe)
            .NotEmpty().WithMessage("Nama tipe wajib diisi.")
            .MaximumLength(100).WithMessage("Nama tipe maksimal 100 karakter.");

        RuleFor(x => x.HargaPerBulan)
            .GreaterThan(0).WithMessage("Harga per bulan harus lebih dari 0.");
    }
}

public class UpdateRoomTypeRequestValidator : AbstractValidator<UpdateRoomTypeRequest>
{
    public UpdateRoomTypeRequestValidator()
    {
        RuleFor(x => x.NamaTipe)
            .NotEmpty().WithMessage("Nama tipe wajib diisi.")
            .MaximumLength(100).WithMessage("Nama tipe maksimal 100 karakter.");

        RuleFor(x => x.HargaPerBulan)
            .GreaterThan(0).WithMessage("Harga per bulan harus lebih dari 0.");
    }
}
