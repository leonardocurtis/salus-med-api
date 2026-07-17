namespace SalusMedApi.Application.Interfaces.Persistence;

public interface IEmployeeRepository
{
    Task<bool> CpfExistsAsync(string cpf);
}
