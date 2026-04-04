using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;
using SalusMedApi.Infrastructure.Repositories.Interfaces;

namespace SalusMedApi.Domain.Entities;

public class Patient : IAuditable
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Cpf { get; set; }
    public Gender Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Address Address { get; set; }
    public PatientStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }
}
