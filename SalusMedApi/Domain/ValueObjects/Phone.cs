using SalusMedApi.Application.Exceptions;

namespace SalusMedApi.Domain.ValueObjects;

public sealed record Phone
{
    public string Value { get; }

    private Phone(string value) => Value = value;

    public static Phone Create(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new DomainException("Phone cannot be empty.");

        var digits = new string(phone.Where(char.IsDigit).ToArray());

        if (digits.Length is not (10 or 11))
            throw new DomainException(
                "Phone must have 10 digits (landline) or 11 digits (mobile), including DDD."
            );

        var firstDigitAfterDdd = digits[2];

        if (digits.Length == 11 && firstDigitAfterDdd != '9')
            throw new DomainException("Mobile phone numbers must start with '9' after the DDD.");

        if (digits.Length == 10 && firstDigitAfterDdd == '9')
            throw new DomainException(
                "A 10-digit phone number cannot start with '9' after the DDD."
            );

        return new Phone(digits);
    }

    public string Ddd => Value[..2];

    public bool IsMobile => Value.Length == 11;

    public string Formatted =>
        IsMobile ? $"({Ddd}) {Value[2..7]}-{Value[7..]}" : $"({Ddd}) {Value[2..6]}-{Value[6..]}";

    public override string ToString() => Value;
}
