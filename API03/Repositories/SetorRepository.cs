using API03.Infra;
using API03.Models;
using Microsoft.EntityFrameworkCore;

namespace API03.Repositories;

// Interface declarada junto no mesmo arquivo
public interface ISetorRepository
{
    Task<IEnumerable<Setor>> GetAllAsync();
    Task<Setor?> GetByIdAsync(int id);
    Task<Setor> AddAsync(Setor setor);
    Task<bool> UpdateAsync(Setor setor);
    Task<bool> DeleteAsync(int id);
}

// Implementação do repositório
public class SetorRepository : ISetorRepository
{
    private readonly AppDbContext _context;

    public SetorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Setor>> GetAllAsync()
    {
        return await _context.Setores
            .Include(s => s.Funcionarios)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Setor?> GetByIdAsync(int id)
    {
        return await _context.Setores
            .Include(s => s.Funcionarios)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Setor> AddAsync(Setor setor)
    {
        await _context.Setores.AddAsync(setor);
        await _context.SaveChangesAsync();
        return setor;
    }

    public async Task<bool> UpdateAsync(Setor setor)
    {
        var exists = await _context.Setores.AnyAsync(s => s.Id == setor.Id);
        if (!exists)
            return false;

        _context.Setores.Update(setor);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var setor = await _context.Setores.FindAsync(id);
        if (setor == null)
            return false;

        _context.Setores.Remove(setor);
        await _context.SaveChangesAsync();
        return true;
    }
}