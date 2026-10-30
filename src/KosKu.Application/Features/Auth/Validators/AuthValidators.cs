using FluentValidation;
using KosKu.Application.Features.Auth.DTOs;

namespace KosKu.Application.Features.Auth.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Nama)
            .NotEmpty().WithMessage("Nama wajib diisi.")
            .MaximumLength(100).WithMessage("Nama maksimal 100 karakter.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email wajib diisi.")
            .EmailAddress().WithMessage("Format email tidak valid.")
            .MaximumLength(150).WithMessage("Email maksimal 150 karakter.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password wajib diisi.")
            .MinimumLength(8).WithMessage("Password minimal 8 karakter.")
            .Matches(@"[A-Z]").WithMessage("Password harus mengandung huruf kapital.")
            .Matches(@"[0-9]").WithMessage("Password harus mengandung angka.");

        RuleFor(x => x.NoHp)
            .MaximumLength(20).WithMessage("Nomor HP maksimal 20 karakter.")
            .When(x => x.NoHp is not null);
    }
}

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email wajib diisi.")
            .EmailAddress().WithMessage("Format email tidak valid.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password wajib diisi.");
    }
}
