using API03.Models;
using API03.Repositories;

namespace API03.Services;

public interface ISetorService
{
    Task<List<Setor>> GetAllAsync();
    Task<Setor?> GetByIdAsync(int id);
    Task<Setor> AddAsync(Setor setor);
    Task<bool> UpdateAsync(int id, Setor setor);
    Task<(bool Removido, string? Erro)> DeleteAsync(int id);
}

public class SetorService : ISetorService
{
    private readonly ISetorRepository _setorRepository;

    public SetorService(ISetorRepository setorRepository)
    {
        _setorRepository = setorRepository;
    }

    public async Task<List<Setor>> GetAllAsync()
    {
        var setores = await _setorRepository.GetAllAsync();
        return setores.ToList();
    }

    public async Task<Setor?> GetByIdAsync(int id)
    {
        return await _setorRepository.GetByIdAsync(id);
    }

    public async Task<Setor> AddAsync(Setor setor)
    {
        setor.Id = 0;
        if (string.IsNullOrWhiteSpace(setor.Nome))
            throw new ArgumentException("O nome do setor é obrigatório.");

        return await _setorRepository.AddAsync(setor);
    }

    public async Task<bool> UpdateAsync(int id, Setor setor)
    {
        if (id != setor.Id)
            return false;

        var existe = await _setorRepository.GetByIdAsync(id);
        if (existe == null)
            return false;

        return await _setorRepository.UpdateAsync(setor);
    }

    public async Task<(bool Removido, string? Erro)> DeleteAsync(int id)
    {
        var setor = await _setorRepository.GetByIdAsync(id);
        if (setor == null)
            return (false, "Setor não encontrado.");

        // Regra de integridade: não permite deletar se houver funcionários vinculados
        if (setor.Funcionarios != null && setor.Funcionarios.Any())
            return (false, "Não é possível excluir um setor que possui funcionários vinculados.");

        var removido = await _setorRepository.DeleteAsync(id);
        if (!removido)
            return (false, "Erro ao remover o setor.");

        return (true, null);
    }
}