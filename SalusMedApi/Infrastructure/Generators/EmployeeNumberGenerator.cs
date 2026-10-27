using System.Globalization;
using Microsoft.EntityFrameworkCore;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.Infrastructure.Persistence;

namespace SalusMedApi.Infrastructure.Generators;

public sealed class EmployeeNumberGenerator(AppDbContext context) : IEmployeeNumberGenerator
{
    public async Task<string> GenerateAsync(CancellationToken ct = default)
    {
        var next = await context
            .Database.SqlQueryRaw<long>("SELECT nextval('employee_number_seq') AS \"Value\"")
            .FirstAsync(ct);

        return FromSequence(next);
    }

    public static string FromSequence(long sequenceValue)
    {
        var payload = sequenceValue.ToString(CultureInfo.InvariantCulture);
        return payload + ComputeCheckDigit(payload);
    }

    public static bool IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length < 2 || !value.All(char.IsAsciiDigit))
            return false;

        return ComputeCheckDigit(value[..^1]) == value[^1] - '0';
    }

    private static int ComputeCheckDigit(string payload)
    {
        var sum = 0;
        var doubleIt = true;

        for (var i = payload.Length - 1; i >= 0; i--)
        {
            var digit = payload[i] - '0';
            if (doubleIt)
            {
                digit *= 2;
                if (digit > 9)
                    digit -= 9;
            }
            sum += digit;
            doubleIt = !doubleIt;
        }

        return (10 - sum % 10) % 10;
    }
}
