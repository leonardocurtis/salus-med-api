namespace SalusMedApi.Infrastructure.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<bool> CpfExistsAsync(string cpf);
}
