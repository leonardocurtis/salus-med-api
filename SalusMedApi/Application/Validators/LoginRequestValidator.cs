using FluentValidation;
using SalusMedApi.Application.DTOs.Auth;
using SalusMedApi.Infrastructure.Generators;

namespace SalusMedApi.Application.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Username is required.")
            .MinimumLength(2)
            .WithMessage("Username must contain at least 2 digits.")
            .Must(EmployeeNumberGenerator.IsValid)
            .WithMessage("Username must be a valid employee number.");

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
