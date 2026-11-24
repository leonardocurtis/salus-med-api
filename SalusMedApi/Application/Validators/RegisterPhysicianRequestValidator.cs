using FluentValidation;
using SalusMedApi.Application.DTOs.Physician;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.Validators;

public class RegisterPhysicianRequestValidator : AbstractValidator<RegisterPhysicianRequest>
{
    private readonly TimeProvider _timeProvider;

    private DateOnly Today => DateOnly.FromDateTime(_timeProvider.GetLocalNow().DateTime);

    public RegisterPhysicianRequestValidator(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(v => v.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters.");
        RuleFor(v => v.Phone)
            .NotEmpty()
            .WithMessage("Phone is required.")
            .Must(Phone.IsValid)
            .WithMessage("Phone must contain only digits, with DDD (e.g., 11912345678).")
            .Must(p => p.Length == 11)
            .WithMessage("Phone must be a mobile number with 11 digits (e.g., 11912345678).");
        RuleFor(v => v.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .MaximumLength(Email.MaxLength)
            .WithMessage($"Email must not exceed {Email.MaxLength} characters.")
            .Must(e => e.LastIndexOf('@') <= Email.MaxLocalLength)
            .WithMessage(
                $"Email local part (before the @) must not exceed {Email.MaxLocalLength} characters."
            )
            .Must(Email.IsValid)
            .WithMessage("Email format is invalid.");
        RuleFor(v => v.CrmNumber)
            .NotEmpty()
            .WithMessage("CRM number is required.")
            .Must(Crm.IsValidNumber)
            .WithMessage(
                $"CRM number must contain {Crm.MinNumberLength} to {Crm.MaxNumberLength} digits, without mask."
            );

        RuleFor(v => v.CrmState)
            .NotNull()
            .WithMessage("CRM state is required.")
            .IsInEnum()
            .WithMessage("CRM state (UF) is invalid.");
        RuleFor(v => v.Cpf)
            .NotEmpty()
            .WithMessage("CPF is required.")
            .Must(Cpf.IsValid)
            .WithMessage("Invalid CPF.");
        RuleFor(v => v.Specialty)
            .NotNull()
            .WithMessage("Specialty is required.")
            .IsInEnum()
            .WithMessage("Specialty value is invalid.");
        RuleFor(v => v.DateOfBirth)
            .NotEmpty()
            .WithMessage("Date of birth is required.")
            .LessThan(_ => Today)
            .WithMessage("Date of birth must be in the past.")
            .GreaterThan(_ => Today.AddYears(-120))
            .WithMessage("Date of birth is not within a valid range.")
            .Must(BeAtLeast18YearsOld)
            .WithMessage("The physician must be at least 18 years old.");
        RuleFor(v => v.DepartmentId)
            .NotEmpty()
            .WithMessage("Department Id must be a valid number.");

        RuleFor(v => v.Address).NotNull().SetValidator(new AddressRequestValidator());
    }

    private bool BeAtLeast18YearsOld(DateOnly? dateOfBirth)
    {
        if (dateOfBirth is null)
            return false;

        var birth = dateOfBirth.Value;

        var age = Today.Year - birth.Year;

        if (birth.AddYears(age) > Today)
            age--;

        return age >= 18;
    }
}
