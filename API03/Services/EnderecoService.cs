using API03.Models;
using API03.Repositories;

namespace API03.Services;

public interface IEnderecoService
{
    Task<List<Endereco>> FindByFuncionarioAsync(int funcionarioId);
    Task<Endereco?> GetByIdAsync(int id);
    Task<(Endereco? Endereco, string? Erro)> AddAsync(int funcionarioId, Endereco endereco);
    Task<(bool Sucesso, string? Erro)> UpdateAsync(int id, Endereco endereco);
    Task<(bool Removido, string? Erro)> DeleteAsync(int id);
}

public class EnderecoService : IEnderecoService
{
    private readonly IEnderecoRepository _enderecoRepository;
    private readonly IFuncionarioRepository _funcionarioRepository;

    public EnderecoService(
        IEnderecoRepository enderecoRepository,
        IFuncionarioRepository funcionarioRepository)
    {
        _enderecoRepository = enderecoRepository;
        _funcionarioRepository = funcionarioRepository;
    }

    public async Task<List<Endereco>> FindByFuncionarioAsync(int funcionarioId)
    {
        return await _enderecoRepository.FindByFuncionarioAsync(funcionarioId);
    }

    public async Task<Endereco?> GetByIdAsync(int id)
    {
        return await _enderecoRepository.GetByIdAsync(id);
    }

    public async Task<(Endereco? Endereco, string? Erro)> AddAsync(int funcionarioId, Endereco endereco)
    {
        // 1. Regra de Integridade: Valida se o funcionário realmente existe no banco
        var funcionarioExiste = await _funcionarioRepository.GetByIdAsync(funcionarioId);
        if (funcionarioExiste == null)
            return (null, $"Operação cancelada: Funcionário com ID {funcionarioId} não existe.");

        // 2. Validações dos campos do endereço
        if (string.IsNullOrWhiteSpace(endereco.Rua))
            return (null, "A rua é obrigatória.");

        if (string.IsNullOrWhiteSpace(endereco.Cidade))
            return (null, "A cidade é obrigatória.");

        if (string.IsNullOrWhiteSpace(endereco.Estado))
            return (null, "O estado é obrigatório.");

        // 3. Força a amarração do ID do funcionário na entidade
        endereco.FuncionarioId = funcionarioId;

        var novoEndereco = await _enderecoRepository.AddAsync(endereco);
        return (novoEndereco, null);
    }

    public async Task<(bool Sucesso, string? Erro)> UpdateAsync(int id, Endereco endereco)
    {
        if (id != endereco.Id)
            return (false, "O ID da rota não coincide com o ID do endereço.");

        // Validação de existência do endereço
        var enderecoExistente = await _enderecoRepository.GetByIdAsync(id);
        if (enderecoExistente == null)
            return (false, "Endereço não encontrado.");

        // Validação de integridade do funcionário vinculado
        var funcionarioExiste = await _funcionarioRepository.GetByIdAsync(endereco.FuncionarioId);
        if (funcionarioExiste == null)
            return (false, $"Funcionário com ID {endereco.FuncionarioId} não foi encontrado.");

        if (string.IsNullOrWhiteSpace(endereco.Rua))
            return (false, "A rua é obrigatória.");

        if (string.IsNullOrWhiteSpace(endereco.Cidade))
            return (false, "A cidade é obrigatória.");

        var atualizado = await _enderecoRepository.UpdateAsync(endereco);
        return (atualizado, atualizado ? null : "Erro ao atualizar endereço.");
    }

    public async Task<(bool Removido, string? Erro)> DeleteAsync(int id)
    {
        var endereco = await _enderecoRepository.GetByIdAsync(id);
        if (endereco == null)
            return (false, "Endereço não encontrado.");

        var removido = await _enderecoRepository.DeleteAsync(id);
        if (!removido)
            return (false, "Erro ao excluir endereço.");

        return (true, null);
    }
}