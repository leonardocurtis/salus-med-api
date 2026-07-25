using Microsoft.EntityFrameworkCore;
using SalusMedApi.Application.Interfaces.Security;
using SalusMedApi.Domain.Constants;
using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Infrastructure.Persistence.Seeders;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context, IPasswordHasher passwordHasher)
    {
        await context.Database.MigrateAsync();

        await SeedRolesAsync(context);
        await SeedAdminUserAsync(context, passwordHasher);
    }

    private static async Task SeedRolesAsync(AppDbContext context)
    {
        var definitions = new[]
        {
            (RoleNames.Admin, "Full system access. Can manage users, clinics and physicians."),
            (RoleNames.Staff, "Authenticated employee. Access based on occupation."),
            (RoleNames.Patient, "Authenticated patient. Access limited to own data."),
        };

        foreach (var (name, description) in definitions)
        {
            var exists = await context.Roles.AnyAsync(r => r.Name == name);

            if (!exists)
                context.Roles.Add(Role.Create(name, description));
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedAdminUserAsync(
        AppDbContext context,
        IPasswordHasher passwordHasher
    )
    {
        const string username = "26070000000";

        var alreadyExists = await context.Users.AnyAsync(u => u.Username == username);

        if (alreadyExists)
            return;

        var adminRole =
            await context.Roles.FirstOrDefaultAsync(r => r.Name == RoleNames.Admin)
            ?? throw new InvalidOperationException(
                "Admin role not found. SeedRolesAsync must complete before SeedAdminUserAsync."
            );

        var passwordHash = passwordHasher.Hash("Admin@1234!");

        var admin = User.Create(username, passwordHash);

        admin.AssignRole(adminRole);

        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}
