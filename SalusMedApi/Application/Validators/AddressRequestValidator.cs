using FluentValidation;
using SalusMedApi.Application.DTOs.Address;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.Validators;

public class AddressRequestValidator : AbstractValidator<AddressRequest>
{
    public AddressRequestValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage("Street is required.")
            .MaximumLength(Address.MaxFieldLength)
            .WithMessage($"Street must not exceed {Address.MaxFieldLength} characters.");

        RuleFor(x => x.Number)
            .NotEmpty()
            .WithMessage("Number is required.")
            .MaximumLength(Address.MaxNumberLength)
            .WithMessage("Number must not exceed 10 characters.")
            .Matches(@"^[a-zA-Z0-9 /]+\z")
            .WithMessage("Number format is invalid.");

        RuleFor(x => x.Complement)
            .MaximumLength(Address.MaxFieldLength)
            .WithMessage("Complement must not exceed 100 characters.")
            .Must(c => c is null || !string.IsNullOrWhiteSpace(c))
            .WithMessage("Complement cannot be empty or whitespace.");

        RuleFor(x => x.Neighborhood)
            .NotEmpty()
            .WithMessage("Neighborhood is required.")
            .MaximumLength(Address.MaxFieldLength)
            .WithMessage("Neighborhood must not exceed 100 characters.");

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage("Postal code is required.")
            .Must(PostalCode.IsValid)
            .WithMessage(
                "Postal code must contain exactly 8 digits, without mask (e.g., 90000000)."
            );

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required.")
            .MaximumLength(Address.MaxFieldLength)
            .WithMessage("City must not exceed 100 characters.");

        RuleFor(x => x.State)
            .NotNull()
            .WithMessage("State is required.")
            .IsInEnum()
            .WithMessage("Invalid state.");
    }
}
