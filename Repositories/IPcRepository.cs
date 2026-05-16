using ComputerManagementApi.Models;

namespace ComputerManagementApi.Repositories;

public interface IPcRepository
{
    Task<IEnumerable<Pc>> GetAllAsync();
    Task<Pc?> GetByIdWithComponentsAsync(int id);
    Task<Pc> AddAsync(Pc pc);
    Task<Pc?> UpdateAsync(int id, Pc pc);
    Task<bool> DeleteAsync(int id);
}
