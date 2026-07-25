using Microsoft.EntityFrameworkCore;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.CrossCutting.Extensions;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Infrastructure.Persistence;

namespace SalusMedApi.Infrastructure.Generators;

public sealed class EmployeeNumberGenerator : IEmployeeNumberGenerator
{
    private readonly AppDbContext _context;

    public EmployeeNumberGenerator(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateAsync(CancellationToken cancellationToken = default)
    {
        var nextValue = await _context
            .Database.SqlQueryRaw<long>("SELECT nextval('employee_number_seq')")
            .FirstAsync(cancellationToken);

        var now = DateTime.UtcNow;

        var year = now.Year % 100;
        var month = now.Month;

        return $"{year:D2}{month:D2}{nextValue:D7)}";
    }
}
