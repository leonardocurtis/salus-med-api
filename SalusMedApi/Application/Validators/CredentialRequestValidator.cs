using FluentValidation;
using SalusMedApi.Application.DTOs.Auth;

namespace SalusMedApi.Application.Validators;

public class CredentialRequestValidator : AbstractValidator<CredentialRequest>
{
    public CredentialRequestValidator()
    {
        RuleFor(v => v.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email format is invalid.");
        RuleFor(v => v.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters.")
            .Matches("[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]")
            .WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]")
            .WithMessage("Password must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]")
            .WithMessage("Password must contain at least one special character.")
            .MaximumLength(64)
            .WithMessage("Password must not exceed 64 characters.");
    }
}
