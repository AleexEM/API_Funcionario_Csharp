using API03.Models;
using API03.Services;
using Microsoft.AspNetCore.Mvc;

namespace API03.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FuncionariosController : ControllerBase
{
    private readonly IFuncionarioService _service;

    public FuncionariosController(IFuncionarioService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Funcionario>>> Listar([FromQuery] int? setorId)
    {
        if (setorId.HasValue)
        {
            var filtrados = await _service.FindBySetorAsync(setorId.Value);
            return Ok(filtrados);
        }

        var todos = await _service.GetAllAsync();
        return Ok(todos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Funcionario>> Obter(int id)
    {
        var funcionario = await _service.GetByIdAsync(id);
        return funcionario is null 
            ? NotFound(new { mensagem = "Funcionário não encontrado." }) 
            : Ok(funcionario);
    }

    [HttpPost]
    public async Task<ActionResult<Funcionario>> Criar([FromBody] Funcionario funcionario)
    {
        var (criado, erro) = await _service.AddAsync(funcionario);

        if (erro != null)
            return BadRequest(new { mensagem = erro });

        return CreatedAtAction(nameof(Obter), new { id = criado!.Id }, criado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] Funcionario funcionario)
    {
        var (atualizado, erro) = await _service.UpdateAsync(id, funcionario);

        if (!atualizado)
        {
            return erro == "Funcionário não encontrado."
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

        return erro == "Funcionário não encontrado."
            ? NotFound(new { mensagem = erro })
            : BadRequest(new { mensagem = erro });
    }
}