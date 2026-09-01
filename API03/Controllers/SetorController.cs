using API03.Models;
using API03.Services;
using Microsoft.AspNetCore.Mvc;

namespace API03.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SetoresController : ControllerBase
{
    private readonly ISetorService _service;

    public SetoresController(ISetorService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Setor>>> Listar() =>
        Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Setor>> Obter(int id)
    {
        var setor = await _service.GetByIdAsync(id);
        return setor is null ? NotFound(new { mensagem = "Setor não encontrado." }) : Ok(setor);
    }

    [HttpPost]
    public async Task<ActionResult<Setor>> Criar([FromBody] Setor setor)
    {
        try
        {
            var criado = await _service.AddAsync(setor);
            return CreatedAtAction(nameof(Obter), new { id = criado.Id }, criado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] Setor setor)
    {
        var atualizado = await _service.UpdateAsync(id, setor);
        return atualizado ? NoContent() : NotFound(new { mensagem = "Setor não encontrado ou dados inválidos." });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover(int id)
    {
        var (removido, erro) = await _service.DeleteAsync(id);

        if (removido)
            return NoContent();

        return erro == "Setor não encontrado." 
            ? NotFound(new { mensagem = erro }) 
            : BadRequest(new { mensagem = erro });
    }
}