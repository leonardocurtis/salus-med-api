using System.Text.RegularExpressions;
using SalusMedApi.Application.Exceptions;

namespace SalusMedApi.Domain.ValueObjects;

public sealed record Email
{
    public string Value { get; }

    private Email(string value) => Value = value;

    private static readonly Regex _regex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email cannot be empty.");

        var normalized = email.Trim().ToLowerInvariant();

        if (normalized.Length > 254)
            throw new DomainException("Email cannot exceed 254 characters.");

        if (!_regex.IsMatch(normalized))
            throw new DomainException($"'{normalized}' is not a valid email address.");

        return new Email(normalized);
    }

    public override string ToString() => Value;
}
