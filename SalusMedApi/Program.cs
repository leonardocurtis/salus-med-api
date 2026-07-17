using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi;
using SalusMedApi.Application.Interfaces.Security;
using SalusMedApi.CrossCutting.ExceptionHandlers;
using SalusMedApi.CrossCutting.Extensions;
using SalusMedApi.Infrastructure.Persistence;
using SalusMedApi.Infrastructure.Persistence.Seeders;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddControllersWithDefaults() // Controllers + JSON
    .AddDatabase(builder.Configuration) // DbContext + connection string
    .AddJwtAuthentication(builder.Configuration) // JWT
    .AddAuthorization()
    .AddApplication() // Services
    .AddInfrastructure() // Repositories + Security
    .AddValidatorsFromAssemblyContaining<Program>() // FluentValidation
    .AddFluentValidationAutoValidation() // FluentValidation
    .AddExceptionHandler<ValidationExceptionHandler>() // Exception
    .AddExceptionHandler<GlobalExceptionHandler>() // Exception handlers
    .AddProblemDetails() // ProblemDetails
    .AddSwaggerGen(opt =>
    {
        opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Salus Med Api", Version = "v1" });
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

    await DbSeeder.SeedAsync(context, passwordHasher);
}

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
