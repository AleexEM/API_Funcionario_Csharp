using API03.Infra;
using API03.Models;
using Microsoft.EntityFrameworkCore;

namespace API03.Repositories;

public interface IEnderecoRepository
{
    Task<List<Endereco>> FindByFuncionarioAsync(int funcionarioId);
    Task<Endereco?> GetByIdAsync(int id);
    Task<Endereco> AddAsync(Endereco endereco);
    Task<bool> UpdateAsync(Endereco endereco);
    Task<bool> DeleteAsync(int id);
}

public class EnderecoRepository : IEnderecoRepository
{
    private readonly AppDbContext _context;

    public EnderecoRepository(AppDbContext dbcontext)
    {
        _context = dbcontext;
    }

    public async Task<List<Endereco>> FindByFuncionarioAsync(int funcionarioId)
    {
        return await _context.Enderecos
            .AsNoTracking()
            .Where(e => e.FuncionarioId == funcionarioId)
            .ToListAsync();
    }

    public async Task<Endereco?> GetByIdAsync(int id)
    {
        return await _context.Enderecos
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Endereco> AddAsync(Endereco endereco)
    {
        await _context.Enderecos.AddAsync(endereco);
        await _context.SaveChangesAsync();
        return endereco;
    }

    public async Task<bool> UpdateAsync(Endereco endereco)
    {
        var exists = await _context.Enderecos.AnyAsync(e => e.Id == endereco.Id);
        if (!exists)
            return false;

        _context.Enderecos.Update(endereco);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var endereco = await _context.Enderecos.FindAsync(id);
        if (endereco == null)
            return false;

        _context.Enderecos.Remove(endereco);
        await _context.SaveChangesAsync();
        return true;
    }
}