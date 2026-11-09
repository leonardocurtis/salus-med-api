using FluentValidation;
using SalusMedApi.Application.DTOs.Employee;

namespace SalusMedApi.Application.Validators;

public class CreateEmployeeCredentialsRequestValidator
    : AbstractValidator<CreateEmployeeCredentialsRequest>
{
    public CreateEmployeeCredentialsRequestValidator()
    {
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage("Passwords do not match.");
    }
}
