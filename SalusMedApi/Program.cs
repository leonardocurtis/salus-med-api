using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi;
using SalusMedApi.CrossCutting.ExceptionHandlers;
using SalusMedApi.CrossCutting.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddControllersWithDefaults() // Controllers + JSON
    .AddDatabase(builder.Configuration) // DbContext + connection string
    .AddJwtAuthentication(builder.Configuration) // JWT
    .AddAuthorization()
    .AddApplication() // Services
    .AddInfrastructure() // Repositories
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
