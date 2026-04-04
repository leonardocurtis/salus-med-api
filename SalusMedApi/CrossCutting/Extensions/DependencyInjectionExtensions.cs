using SalusMedApi.Application.Services;
using SalusMedApi.Application.Services.Interfaces;
using SalusMedApi.Infrastructure.Repositories;
using SalusMedApi.Infrastructure.Repositories.Interfaces;

namespace SalusMedApi.CrossCutting.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IPhysicianRepository, PhysicianRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}
