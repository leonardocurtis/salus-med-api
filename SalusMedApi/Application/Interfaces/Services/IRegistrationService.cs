namespace SalusMedApi.Application.Interfaces.Services;

public interface IRegistrationService<TRequest, TResponse>
{
    Task<TResponse> RegisterAsync(TRequest request, CancellationToken cancellationToken = default);
}
