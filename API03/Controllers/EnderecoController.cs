using API03.Models;
using API03.Services;
using Microsoft.AspNetCore.Mvc;

namespace API03.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnderecosController : ControllerBase
{
    private readonly IEnderecoService _service;

    public EnderecosController(IEnderecoService service)
    {
        _service = service;
    }

    [HttpGet("funcionario/{funcionarioId:int}")]
    public async Task<ActionResult<List<Endereco>>> ListarPorFuncionario(int funcionarioId)
    {
        var enderecos = await _service.FindByFuncionarioAsync(funcionarioId);
        return Ok(enderecos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Endereco>> Obter(int id)
    {
        var endereco = await _service.GetByIdAsync(id);
        return endereco is null 
            ? NotFound(new { mensagem = "Endereço não encontrado." }) 
            : Ok(endereco);
    }

    [HttpPost("funcionario/{funcionarioId:int}")]
    public async Task<ActionResult<Endereco>> Criar(int funcionarioId, [FromBody] Endereco endereco)
    {
        var (criado, erro) = await _service.AddAsync(funcionarioId, endereco);

        if (erro != null)
            return BadRequest(new { mensagem = erro });

        return CreatedAtAction(nameof(Obter), new { id = criado!.Id }, criado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] Endereco endereco)
    {
        var (atualizado, erro) = await _service.UpdateAsync(id, endereco);

        if (!atualizado)
        {
            return erro == "Endereço não encontrado."
                ? NotFound(new { mensagem = erro })
                : BadRequest(new { mensagem = erro });
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover(int id)
    {
        var (removido, erro) = await _service.DeleteAsync(id);

        if (removido)
            return NoContent();

        return NotFound(new { mensagem = erro ?? "Endereço não encontrado." });
    }
}