using FluentValidation;
using KosKu.Application.Features.Users.DTOs;

namespace KosKu.Application.Features.Users.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Nama)
            .NotEmpty().WithMessage("Nama wajib diisi.")
            .MaximumLength(100).WithMessage("Nama maksimal 100 karakter.");

        RuleFor(x => x.NoHp)
            .MaximumLength(20).WithMessage("Nomor HP maksimal 20 karakter.")
            .When(x => x.NoHp is not null);
    }
}

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.PasswordLama)
            .NotEmpty().WithMessage("Password lama wajib diisi.");

        RuleFor(x => x.PasswordBaru)
            .NotEmpty().WithMessage("Password baru wajib diisi.")
            .MinimumLength(8).WithMessage("Password baru minimal 8 karakter.")
            .Matches(@"[A-Z]").WithMessage("Password harus mengandung huruf kapital.")
            .Matches(@"[0-9]").WithMessage("Password harus mengandung angka.");
    }
}
