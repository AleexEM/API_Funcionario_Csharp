using API03.Models;
using API03.Repositories;

namespace API03.Services;

public interface IFuncionarioService
{
    Task<List<Funcionario>> GetAllAsync();
    Task<List<Funcionario>> FindBySetorAsync(int? setorId);
    Task<Funcionario?> GetByIdAsync(int id);
    Task<(Funcionario? Funcionario, string? Erro)> AddAsync(Funcionario funcionario);
    Task<(bool Sucesso, string? Erro)> UpdateAsync(int id, Funcionario funcionario);
    Task<(bool Removido, string? Erro)> DeleteAsync(int id);
}

public class FuncionarioService : IFuncionarioService
{
    private readonly IFuncionarioRepository _funcionarioRepository;
    private readonly ISetorRepository _setorRepository;

    public FuncionarioService(
        IFuncionarioRepository funcionarioRepository,
        ISetorRepository setorRepository)
    {
        _funcionarioRepository = funcionarioRepository;
        _setorRepository = setorRepository;
    }

    public async Task<List<Funcionario>> GetAllAsync()
    {
        var funcionarios = await _funcionarioRepository.GetAllAsync();
        return funcionarios.ToList();
    }

    public async Task<List<Funcionario>> FindBySetorAsync(int? setorId)
    {
        return await _funcionarioRepository.FindBySetorAsync(setorId);
    }

    public async Task<Funcionario?> GetByIdAsync(int id)
    {
        return await _funcionarioRepository.GetByIdAsync(id);
    }

    public async Task<(Funcionario? Funcionario, string? Erro)> AddAsync(Funcionario funcionario)
    {
        if (string.IsNullOrWhiteSpace(funcionario.Nome))
            return (null, "O nome do funcionário é obrigatório.");

        if (funcionario.Salario <= 0)
            return (null, "O salário deve ser maior que zero.");

        // Validação: setor precisa existir
        var setorExiste = await _setorRepository.GetByIdAsync(funcionario.SetorId);
        if (setorExiste == null)
            return (null, $"O setor com ID {funcionario.SetorId} não existe.");
        funcionario.Enderecos.Clear();

        var novoFuncionario = await _funcionarioRepository.AddAsync(funcionario);
        return (novoFuncionario, null);
    }

    public async Task<(bool Sucesso, string? Erro)> UpdateAsync(int id, Funcionario funcionario)
    {
        if (id != funcionario.Id)
            return (false, "O ID da rota não coincide com o ID do funcionário.");

        if (string.IsNullOrWhiteSpace(funcionario.Nome))
            return (false, "O nome do funcionário é obrigatório.");

        if (funcionario.Salario <= 0)
            return (false, "O salário deve ser maior que zero.");

        var funcionarioExistente = await _funcionarioRepository.GetByIdAsync(id);
        if (funcionarioExistente == null)
            return (false, "Funcionário não encontrado.");

        // Validação: caso altere o setor, o novo setor precisa existir
        var setorExiste = await _setorRepository.GetByIdAsync(funcionario.SetorId);
        if (setorExiste == null)
            return (false, $"O setor com ID {funcionario.SetorId} não existe.");

        var atualizado = await _funcionarioRepository.UpdateAsync(funcionario);
        return (atualizado, atualizado ? null : "Erro ao atualizar funcionário.");
    }

    public async Task<(bool Removido, string? Erro)> DeleteAsync(int id)
    {
        var funcionario = await _funcionarioRepository.GetByIdAsync(id);
        if (funcionario == null)
            return (false, "Funcionário não encontrado.");

        var removido = await _funcionarioRepository.DeleteAsync(id);
        if (!removido)
            return (false, "Erro ao excluir funcionário.");

        return (true, null);
    }
}