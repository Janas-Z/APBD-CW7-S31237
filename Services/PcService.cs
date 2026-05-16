using ComputerManagementApi.DTOs;
using ComputerManagementApi.Models;
using ComputerManagementApi.Repositories;

namespace ComputerManagementApi.Services;

public class PcService : IPcService
{
    private readonly IPcRepository _repository;

    public PcService(IPcRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PcListDto>> GetAllAsync()
    {
        var pcs = await _repository.GetAllAsync();
        return pcs.Select(MapToListDto);
    }

    public async Task<PcWithComponentsDto?> GetByIdWithComponentsAsync(int id)
    {
        var pc = await _repository.GetByIdWithComponentsAsync(id);
        if (pc is null) return null;

        return new PcWithComponentsDto
        {
            Id        = pc.Id,
            Name      = pc.Name,
            Weight    = pc.Weight,
            Warranty  = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock     = pc.Stock,
            Components = pc.PcComponents.Select(pc => new PcComponentDto
            {
                Amount = pc.Amount,
                Component = new ComponentDto
                {
                    Code        = pc.Component.Code,
                    Name        = pc.Component.Name,
                    Description = pc.Component.Description,
                    Manufacturer = new ManufacturerDto
                    {
                        Id             = pc.Component.Manufacturer.Id,
                        Abbreviation   = pc.Component.Manufacturer.Abbreviation,
                        FullName       = pc.Component.Manufacturer.FullName,
                        FoundationDate = pc.Component.Manufacturer.FoundationDate
                    },
                    Type = new ComponentTypeDto
                    {
                        Id           = pc.Component.Type.Id,
                        Abbreviation = pc.Component.Type.Abbreviation,
                        Name         = pc.Component.Type.Name
                    }
                }
            }).ToList()
        };
    }

    public async Task<PcListDto> CreateAsync(PcCreateDto dto)
    {
        var pc = new Pc
        {
            Name      = dto.Name,
            Weight    = dto.Weight,
            Warranty  = dto.Warranty,
            CreatedAt = dto.CreatedAt,
            Stock     = dto.Stock
        };

        var created = await _repository.AddAsync(pc);
        return MapToListDto(created);
    }

    public async Task<PcListDto?> UpdateAsync(int id, PcUpdateDto dto)
    {
        var updated = new Pc
        {
            Name      = dto.Name,
            Weight    = dto.Weight,
            Warranty  = dto.Warranty,
            CreatedAt = dto.CreatedAt,
            Stock     = dto.Stock
        };

        var result = await _repository.UpdateAsync(id, updated);
        return result is null ? null : MapToListDto(result);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static PcListDto MapToListDto(Pc pc) => new()
    {
        Id        = pc.Id,
        Name      = pc.Name,
        Weight    = pc.Weight,
        Warranty  = pc.Warranty,
        CreatedAt = pc.CreatedAt,
        Stock     = pc.Stock
    };
}
