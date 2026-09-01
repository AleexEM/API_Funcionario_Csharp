using API03.Models;
using API03.Services;
using Microsoft.AspNetCore.Mvc;

namespace API03.Controllers;

[ApiController]
[Route("api/funcionario/{funcionarioId:int}/[controller]")]
public class EnderecosController : ControllerBase
{
    private readonly IEnderecoService _service;

    public EnderecosController(IEnderecoService service)
    {
        _service = service;
    }

    // GET: /api/funcionario/1/enderecos
    [HttpGet]
    public async Task<ActionResult<List<Endereco>>> ListarPorFuncionario(int funcionarioId)
    {
        var enderecos = await _service.FindByFuncionarioAsync(funcionarioId);
        return Ok(enderecos);
    }

    // GET: /api/funcionario/1/enderecos/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Endereco>> Obter(int funcionarioId, int id)
    {
        var endereco = await _service.GetByIdAsync(id);
        
        if (endereco is null || endereco.FuncionarioId != funcionarioId)
            return NotFound(new { mensagem = "Endereço não encontrado para este funcionário." });

        return Ok(endereco);
    }

    // POST: /api/funcionario/1/enderecos
    [HttpPost]
    public async Task<ActionResult<Endereco>> Criar(int funcionarioId, [FromBody] Endereco endereco)
    {
        var (criado, erro) = await _service.AddAsync(funcionarioId, endereco);

        if (erro != null)
            return BadRequest(new { mensagem = erro });

        return CreatedAtAction(nameof(Obter), new { funcionarioId, id = criado!.Id }, criado);
    }

    // PUT: /api/funcionario/1/enderecos/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int funcionarioId, int id, [FromBody] Endereco endereco)
    {
        endereco.FuncionarioId = funcionarioId;
        var (atualizado, erro) = await _service.UpdateAsync(id, endereco);

        if (!atualizado)
        {
            return erro == "Endereço não encontrado."
                ? NotFound(new { mensagem = erro })
                : BadRequest(new { mensagem = erro });
        }

        return NoContent();
    }

    // DELETE: /api/funcionario/1/enderecos/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover(int funcionarioId, int id)
    {
        var endereco = await _service.GetByIdAsync(id);
        if (endereco == null || endereco.FuncionarioId != funcionarioId)
            return NotFound(new { mensagem = "Endereço não encontrado para este funcionário." });

        var (removido, erro) = await _service.DeleteAsync(id);

        if (removido)
            return NoContent();

        return BadRequest(new { mensagem = erro ?? "Erro ao remover endereço." });
    }
}