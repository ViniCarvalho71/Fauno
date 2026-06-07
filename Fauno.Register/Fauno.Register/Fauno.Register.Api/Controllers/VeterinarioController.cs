using Fauno.Register.Application.DTOs.Requests;
using Fauno.Register.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fauno.Register.Api.Controllers;

[ApiController]
[Route("api/cadastros/veterinarios")]
public class VeterinarioController : ControllerBase
{
    private readonly CadastrarVeterinarioUseCase _cadastrarVeterinarioUseCase;
    private readonly ObterVeterinarioIdPorUserIdUseCase _obterVetIdUseCase;
    private readonly VerificarVeterinarioExisteUseCase _verificarVetExisteUseCase;

    public VeterinarioController(CadastrarVeterinarioUseCase cadastrarVeterinarioUseCase, 
        ObterVeterinarioIdPorUserIdUseCase obterVetIdUseCase,
        VerificarVeterinarioExisteUseCase verificarVeterinarioExisteUseCase)
    {
        _cadastrarVeterinarioUseCase = cadastrarVeterinarioUseCase;
        _obterVetIdUseCase = obterVetIdUseCase;
        _verificarVetExisteUseCase = verificarVeterinarioExisteUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] CadastrarVeterinarioRequest request)
    {
        try
        {
            var vet = await _cadastrarVeterinarioUseCase.Run(request);
            return Created($"/api/cadastros/veterinarios/{vet.Id}", new { vet.Id, vet.Nome, Cpf = vet.Cpf.Numero });
        }
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }
    [Authorize]
    [HttpGet("usuario/{userId}/id")]
    public async Task<IActionResult> GetVeterinarianIdByUserId(Guid userId)
    {
        var id = await _obterVetIdUseCase.Run(userId);
        if (id == null) return NotFound(new { mensagem = "Veterinário não encontrado para este usuário." });
    
        return Ok(new { veterinarianId = id });
    }
    [Authorize]
    [HttpGet("usuario/{veterinarianId}/existe")]
    public async Task<IActionResult> VeterinarianExists(Guid veterinarianId)
    {
        var existe = await _verificarVetExisteUseCase.Run(veterinarianId);
        return Ok(existe);
    }
}