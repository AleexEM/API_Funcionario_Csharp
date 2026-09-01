using API03.Infra;
using API03.Models;
using Microsoft.EntityFrameworkCore;

namespace API03.Repositories;

public interface IFuncionarioRepository
{
    Task<IEnumerable<Funcionario>> GetAllAsync();
    Task<List<Funcionario>> FindBySetorAsync(int? setorId);
    Task<Funcionario?> GetByIdAsync(int id);
    Task<Funcionario> AddAsync(Funcionario funcionario);
    Task<bool> UpdateAsync(Funcionario funcionario);
    Task<bool> DeleteAsync(int id);
}

public class FuncionarioRepository : IFuncionarioRepository
{
    private readonly AppDbContext _context;

    public FuncionarioRepository(AppDbContext dbcontext)
    {
        _context = dbcontext;
    }

    // Método auxiliar reaproveitável com os Includes
    private IQueryable<Funcionario> ComRelacionamento() =>
        _context.Funcionarios
            .Include(f => f.Setor)
            .Include(f => f.Enderecos);

    public async Task<IEnumerable<Funcionario>> GetAllAsync()
    {
        return await ComRelacionamento()
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Funcionario>> FindBySetorAsync(int? setorId)
    {
        var query = ComRelacionamento().AsNoTracking();

        if (setorId.HasValue)
        {
            query = query.Where(f => f.SetorId == setorId.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<Funcionario?> GetByIdAsync(int id)
    {
        return await ComRelacionamento()
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<Funcionario> AddAsync(Funcionario funcionario)
    {
        await _context.Funcionarios.AddAsync(funcionario);
        await _context.SaveChangesAsync();
        return funcionario;
    }

    public async Task<bool> UpdateAsync(Funcionario funcionario)
    {
        var exists = await _context.Funcionarios.AnyAsync(f => f.Id == funcionario.Id);
        if (!exists)
            return false;

        _context.Funcionarios.Update(funcionario);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var funcionario = await _context.Funcionarios.FindAsync(id);
        if (funcionario == null)
            return false;

        _context.Funcionarios.Remove(funcionario);
        await _context.SaveChangesAsync();
        return true;
    }
}