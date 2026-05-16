using ComputerManagementApi.DTOs;

namespace ComputerManagementApi.Services;

public interface IPcService
{
    Task<IEnumerable<PcListDto>> GetAllAsync();
    Task<PcWithComponentsDto?> GetByIdWithComponentsAsync(int id);
    Task<PcListDto> CreateAsync(PcCreateDto dto);
    Task<PcListDto?> UpdateAsync(int id, PcUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
