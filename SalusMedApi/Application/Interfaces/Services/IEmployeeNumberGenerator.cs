using SalusMedApi.Domain.Enums;

namespace SalusMedApi.Application.Interfaces.Services;

public interface IEmployeeNumberGenerator
{
    Task<string> GenerateAsync(CancellationToken cancellationToken = default);
}
