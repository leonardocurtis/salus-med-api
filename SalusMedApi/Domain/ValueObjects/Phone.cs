using SalusMedApi.Domain.Exceptions;

namespace SalusMedApi.Domain.ValueObjects;

public sealed record Phone
{
    public string Value { get; }

    private Phone(string value) => Value = value;

    public static bool IsValid(string? phone) => GetError(phone) is null;

    public static Phone Create(string phone) =>
        GetError(phone) is { } error ? throw new DomainException(error) : new Phone(phone);

    public string Ddd => Value[..2];

    public bool IsMobile => Value.Length == 11;

    public string Formatted =>
        IsMobile ? $"({Ddd}) {Value[2..7]}-{Value[7..]}" : $"({Ddd}) {Value[2..6]}-{Value[6..]}";

    public override string ToString() => Value;

    private static string? GetError(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return "Phone cannot be empty.";

        if (!phone.All(char.IsAsciiDigit))
            return "Phone must contain only digits (no mask or symbols).";

        if (phone.Length is not (10 or 11))
            return "Phone must have 10 digits (landline) or 11 digits (mobile), including DDD.";

        if (phone[0] == '0' || phone[1] == '0')
            return "Phone DDD is invalid.";

        var isMobile = phone.Length == 11;

        if (isMobile && phone[2] != '9')
            return "Mobile phone numbers must start with '9' after the DDD.";

        if (!isMobile && phone[2] is not (>= '2' and <= '5'))
            return "Landline numbers must start with 2, 3, 4 or 5 after the DDD.";

        return null;
    }
}
