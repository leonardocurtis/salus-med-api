using FluentValidation;
using SalusMedApi.Application.DTOs.Patient;

namespace SalusMedApi.Application.Validators;

public class RegisterPatientRequestValidator : AbstractValidator<RegisterPatientRequest>
{
    public RegisterPatientRequestValidator()
    {
        RuleFor(v => v.Patient.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters.");
        RuleFor(v => v.Patient.Phone)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Phone is required.")
            .Matches(@"^[1-9]{2}9\d{8}$")
            .WithMessage(
                "Phone number must be a valid Brazilian mobile number (e.g., 11912345678)."
            );
        RuleFor(v => v.Patient.Cpf)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("CPF is required.")
            .MaximumLength(11)
            .WithMessage("CPF must contain exactly 11 numeric digits.")
            .Must(BeAValidCpf)
            .WithMessage("Invalid CPF.");
        RuleFor(v => v.Patient.Gender)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("Gender is required.")
            .IsInEnum()
            .WithMessage("Gender value is invalid.");
        RuleFor(v => v.Patient.DateOfBirth)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Date of birth is required.")
            .LessThan(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Date of birth must be in the past.")
            .GreaterThan(DateOnly.FromDateTime(DateTime.Today.AddYears(-120)))
            .WithMessage("Date of birth is not within a valid range.");

        RuleFor(v => v.Patient.Address).NotNull().SetValidator(new AddressRequestValidator());

        RuleFor(v => v.Credentials).NotNull().SetValidator(new CredentialRequestValidator());
    }

    private bool BeAValidCpf(string cpf)
    {
        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpf.Length != 11 || cpf.All(c => c == cpf[0]))
            return false;

        var digit1 = CalculateDigit(cpf, 9);
        var digit2 = CalculateDigit(cpf, 10);

        return cpf[9] - '0' == digit1 && cpf[10] - '0' == digit2;
    }

    private int CalculateDigit(string cpf, int length)
    {
        int[] multipliers =
            length == 9 ? [10, 9, 8, 7, 6, 5, 4, 3, 2] : [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

        var sum = cpf.Take(length).Select((c, i) => (c - '0') * multipliers[i]).Sum();
        var remainder = sum % 11;

        return remainder < 2 ? 0 : 11 - remainder;
    }
}
