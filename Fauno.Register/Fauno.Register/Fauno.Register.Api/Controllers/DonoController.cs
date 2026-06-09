using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fauno.Register.Application.UseCases;
using Fauno.Register.Application.DTOs.Requests;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Fauno.Register.Api.Controllers;

[ApiController]
[Route("api/cadastros/donos")]
public class DonoController : ControllerBase
{
    private readonly CadastrarDonoUseCase _cadastrarDonoUseCase;
    private readonly ObterDonoIdPorUserIdUseCase _obterDonoIdUseCase;
    private readonly ObterDonoPorIdUseCase _obterDonoPorIdUseCase;
    private readonly VerificarDonoExisteUseCase _verificarDonoExisteUseCase;

    public DonoController(CadastrarDonoUseCase cadastrarDonoUseCase, ObterDonoIdPorUserIdUseCase obterDonoIdUseCase, ObterDonoPorIdUseCase obterDonoPorIdUseCase, VerificarDonoExisteUseCase verificarDonoExisteUseCase)
    {
        _cadastrarDonoUseCase = cadastrarDonoUseCase;
        _obterDonoIdUseCase = obterDonoIdUseCase;
        _obterDonoPorIdUseCase = obterDonoPorIdUseCase;
        _verificarDonoExisteUseCase = verificarDonoExisteUseCase;

    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] CadastrarDonoRequest request)
    {
        try
        {
            var dono = await _cadastrarDonoUseCase.Run(request);
            return Created($"/api/cadastros/donos/{dono.Id}", new { dono.Id, dono.Nome, Cpf = dono.Cpf.Numero });
        }
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }
    [Authorize]
    [HttpGet("{ownerId}")]
    public async Task<IActionResult> GetOwner(Guid ownerId)
    {
        var dono = await _obterDonoPorIdUseCase.Run(ownerId);
        if (dono == null) return NotFound(new { mensagem = "Dono não encontrado." });

        return Ok(dono);
    }
    [Authorize]
    [HttpGet("usuario/{userId}/id")]
    public async Task<IActionResult> GetOwnerIdByUserId(Guid userId)
    {
        var id = await _obterDonoIdUseCase.Run(userId);
        if (id == null) return NotFound(new { mensagem = "Dono não encontrado para este usuário." });
    
        return Ok(new { ownerId = id });
    }
    [Authorize]
    [HttpGet("usuario/{ownerId}/existe")]
    public async Task<IActionResult> OwnerExists(Guid ownerId)
    {
        var existe = await _verificarDonoExisteUseCase.Run(ownerId);
        return Ok(existe);
    }
}