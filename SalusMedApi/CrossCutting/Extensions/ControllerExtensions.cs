using System.Text.Json;
using System.Text.Json.Serialization;

namespace SalusMedApi.CrossCutting.Extensions;

public static class ControllerExtensions
{
    public static IServiceCollection AddControllersWithDefaults(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

        return services;
    }
}
