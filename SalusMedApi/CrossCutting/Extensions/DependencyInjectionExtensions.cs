using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Application.Interfaces.Security;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.Application.Services;
using SalusMedApi.Infrastructure.Repositories;
using SalusMedApi.Infrastructure.Security;

namespace SalusMedApi.CrossCutting.Extensions;

public static class DependencyInjectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplication()
        {
            services.AddApplicationServices();

            return services;
        }

        public IServiceCollection AddInfrastructure()
        {
            services.AddRepositories();
            services.AddSecurity();

            return services;
        }

        private void AddApplicationServices()
        {
            //services.AddScoped<IPdfService, AppointmentPdfService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
        }

        private void AddRepositories()
        {
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IPhysicianRepository, PhysicianRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        private void AddSecurity()
        {
            services.AddScoped<IPasswordHasher, PasswordHasher>();
        }
    }
}
