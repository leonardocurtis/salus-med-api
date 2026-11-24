using System.Text.RegularExpressions;
using SalusMedApi.Domain.Exceptions;

namespace SalusMedApi.Domain.ValueObjects;

public sealed partial record Email
{
    [GeneratedRegex(
        @"^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)+\z",
        RegexOptions.None,
        matchTimeoutMilliseconds: 100
    )]
    private static partial Regex EmailFormat();

    public const int MaxLength = 254;
    public const int MaxLocalLength = 64;

    public string Value { get; }

    private Email(string value) => Value = value;

    public static Email Create(string email) =>
        GetError(email, out var normalized) is { } error
            ? throw new DomainException(error)
            : new Email(normalized);

    public static bool IsValid(string? email) => GetError(email, out _) is null;

    private static string? GetError(string? email, out string normalized)
    {
        normalized = string.Empty;

        if (string.IsNullOrWhiteSpace(email))
            return "Email cannot be empty.";

        normalized = email.Trim().ToLowerInvariant();

        if (normalized.Length > MaxLength)
            return $"Email cannot exceed {MaxLength} characters.";

        if (normalized.LastIndexOf('@') > MaxLocalLength)
            return $"Email local part cannot exceed {MaxLocalLength} characters.";

        return EmailFormat().IsMatch(normalized) ? null : "Email format is invalid.";
    }

    public override string ToString() => Value;
}
