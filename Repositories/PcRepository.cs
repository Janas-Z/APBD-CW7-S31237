using ComputerManagementApi.Data;
using ComputerManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ComputerManagementApi.Repositories;

public class PcRepository : IPcRepository
{
    private readonly AppDbContext _context;

    public PcRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pc>> GetAllAsync()
    {
        return await _context.Pcs
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Pc?> GetByIdWithComponentsAsync(int id)
    {
        return await _context.Pcs
            .AsNoTracking()
            .Include(p => p.PcComponents)
                .ThenInclude(pc => pc.Component)
                    .ThenInclude(c => c.Manufacturer)
            .Include(p => p.PcComponents)
                .ThenInclude(pc => pc.Component)
                    .ThenInclude(c => c.Type)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Pc> AddAsync(Pc pc)
    {
        _context.Pcs.Add(pc);
        await _context.SaveChangesAsync();
        return pc;
    }

    public async Task<Pc?> UpdateAsync(int id, Pc updated)
    {
        var existing = await _context.Pcs.FindAsync(id);
        if (existing is null) return null;

        existing.Name = updated.Name;
        existing.Weight = updated.Weight;
        existing.Warranty = updated.Warranty;
        existing.CreatedAt = updated.CreatedAt;
        existing.Stock = updated.Stock;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var pc = await _context.Pcs.FindAsync(id);
        if (pc is null) return false;

        _context.Pcs.Remove(pc);
        await _context.SaveChangesAsync();
        return true;
    }
}
