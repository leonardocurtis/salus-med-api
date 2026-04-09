using FluentValidation;
using SalusMedApi.Application.DTOs.Address;

namespace SalusMedApi.Application.Validators;

public class AddressRequestValidator : AbstractValidator<AddressRequest>
{
    public AddressRequestValidator()
    {
        RuleFor(s => s.Street)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Street is required.")
            .MaximumLength(150)
            .WithMessage("Street must not exceed 150 characters.");
        RuleFor(n => n.Number)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Number is required.")
            .MaximumLength(10)
            .WithMessage("Number must not exceed 10 characters.")
            .Matches(@"^[a-zA-Z0-9\s/]+$")
            .WithMessage("Number format is invalid.");
        RuleFor(c => c.Complement)
            .MaximumLength(100)
            .WithMessage("Complement must not exceed 100 characters.")
            .Must(c => c is null || !string.IsNullOrWhiteSpace(c))
            .WithMessage("Complement cannot be empty or whitespace.");
        RuleFor(n => n.Neighborhood)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Neighborhood is required.")
            .MaximumLength(100)
            .WithMessage("Neighborhood must not exceed 100 characters.");
        RuleFor(c => c.City)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("City is required.")
            .MaximumLength(100)
            .WithMessage("City must not exceed 100 characters.");
        RuleFor(s => s.State)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("State is required.")
            .Length(2)
            .WithMessage("State must be a 2-letter code (e.g., RS, SP).")
            .Matches(@"^[A-Z]{2}$")
            .WithMessage("State must contain only uppercase letters (e.g., RS, SP).")
            .Must(state => ValidStates.Contains(state))
            .WithMessage("Invalid state.");
        RuleFor(p => p.PostalCode)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Postal code is required.")
            .Matches(@"^\d{8}$")
            .WithMessage("Postal code must contain exactly 8 numeric digits.");
    }

    private static readonly string[] ValidStates =
    [
        "AC",
        "AL",
        "AP",
        "AM",
        "BA",
        "CE",
        "DF",
        "ES",
        "GO",
        "MA",
        "MT",
        "MS",
        "MG",
        "PA",
        "PB",
        "PR",
        "PE",
        "PI",
        "RJ",
        "RN",
        "RS",
        "RO",
        "RR",
        "SC",
        "SP",
        "SE",
        "TO",
    ];
}
