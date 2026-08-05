using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SalusMedApi.Application.Interfaces.Auth;
using SalusMedApi.Domain.Common;
using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUser)
    : DbContext(options)
{
    public DbSet<Physician> Physicians { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Clinic> Clinics { get; set; }
    public DbSet<HealthUnit> HealthUnits { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        ApplyAuditTimestamps();
        return await base.SaveChangesAsync(ct);
    }

    public override int SaveChanges()
    {
        ApplyAuditTimestamps();
        return base.SaveChanges();
    }

    private void ApplyAuditTimestamps()
    {
        var now = DateTimeOffset.UtcNow;
        var user = currentUser.EmployeeIdNumber ?? "SYSTEM";
        var entries = ChangeTracker.Entries<IAuditable>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property(nameof(IAuditable.CreatedAt)).CurrentValue = now;
                    entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = null;
                    entry.Property(nameof(IAuditable.CreatedBy)).CurrentValue = user;
                    break;

                case EntityState.Modified:
                    entry.Property(nameof(IAuditable.CreatedAt)).IsModified = false;
                    entry.Property(nameof(IAuditable.CreatedBy)).IsModified = false;
                    entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = now;
                    entry.Property(nameof(IAuditable.UpdatedBy)).CurrentValue = user;
                    break;
                case EntityState.Deleted:
                    throw new InvalidOperationException(
                        $"Physical deletion of {entry.Entity.GetType().Name} is not allowed. Use status transitions instead."
                    );
                case EntityState.Detached:
                case EntityState.Unchanged:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
