using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fauno.Register.Application.UseCases;
using Fauno.Register.Application.DTOs.Requests;

namespace Fauno.Register.Api.Controllers;

[ApiController]
[Route("api/cadastros/donos")]
public class DonoController : ControllerBase
{
    private readonly CadastrarDonoUseCase _useCase;
    private readonly ObterDonoIdPorUserIdUseCase _obterDonoIdUseCase;
    private readonly VerificarDonoExisteUseCase _verificarDonoExisteUseCase;

    public DonoController(CadastrarDonoUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] CadastrarDonoRequest request)
    {
        try
        {
            var dono = await _useCase.Run(request);
            return Created($"/api/cadastros/donos/{dono.Id}", new { dono.Id, dono.Nome, Cpf = dono.Cpf.Numero });
        }
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }
    
    [HttpGet("usuario/{userId}/id")]
    public async Task<IActionResult> GetOwnerIdByUserId(Guid userId)
    {
        var id = await _obterDonoIdUseCase.Run(userId);
        if (id == null) return NotFound(new { mensagem = "Dono não encontrado para este usuário." });
    
        return Ok(new { ownerId = id });
    }

    [HttpGet("usuario/{userId}/existe")]
    public async Task<IActionResult> OwnerExists(Guid id)
    {
        var existe = await _verificarDonoExisteUseCase.Run(id);
        return Ok(existe);
    }
}