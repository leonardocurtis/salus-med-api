using SalusMedApi.Application.DTOs.Physician;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Application.Interfaces.Security;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.Application.Services;
using SalusMedApi.Infrastructure.Generators;
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
            services.AddGenerators();

            return services;
        }

        private void AddApplicationServices()
        {
            //services.AddScoped<IPdfService, AppointmentPdfService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<
                IRegistrationService<RegisterPhysicianRequest, RegisterPhysicianResponse>,
                PhysicianService
            >();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IClinicService, ClinicService>();
        }

        private void AddRepositories()
        {
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IPhysicianRepository, PhysicianRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUnitOfWorkRepository, UnitOfWorkRepository>();
            services.AddScoped<IClinicRepository, ClinicRepository>();
        }

        private void AddSecurity()
        {
            services.AddScoped<IPasswordHasher, PasswordHasher>();
        }

        private void AddGenerators()
        {
            services.AddScoped<IEmployeeNumberGenerator, EmployeeNumberGenerator>();
        }
    }
}
