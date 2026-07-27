using FluentValidation;
using SalusMedApi.Application.DTOs.Auth;

namespace SalusMedApi.Application.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Username is required.")
            .Matches(@"^\d{11}$")
            .WithMessage("Username must contain only numeric digits.");
        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters.")
            .MaximumLength(64)
            .WithMessage("Password must not exceed 64 characters.");
    }
}
