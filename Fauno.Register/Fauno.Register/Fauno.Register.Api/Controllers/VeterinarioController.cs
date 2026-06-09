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
    private readonly ObterVeterinarioPorIdUseCase _obterVeterinarioPorIdUseCase;
    private readonly VerificarVeterinarioExisteUseCase _verificarVetExisteUseCase;
    private readonly ObterVeterinariosUseCase _obterVeterinariosUseCase;
    public VeterinarioController(CadastrarVeterinarioUseCase cadastrarVeterinarioUseCase, 
        ObterVeterinarioIdPorUserIdUseCase obterVetIdUseCase,
        ObterVeterinarioPorIdUseCase obterVeterinarioPorIdUseCase,
        VerificarVeterinarioExisteUseCase verificarVeterinarioExisteUseCase,
        ObterVeterinariosUseCase obterVeterinariosUseCase)
    {
        _cadastrarVeterinarioUseCase = cadastrarVeterinarioUseCase;
        _obterVetIdUseCase = obterVetIdUseCase;
        _obterVeterinarioPorIdUseCase = obterVeterinarioPorIdUseCase;
        _verificarVetExisteUseCase = verificarVeterinarioExisteUseCase;
        _obterVeterinariosUseCase = obterVeterinariosUseCase;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> ObterTodos()
    {
        var veterinarios = await _obterVeterinariosUseCase.Run();
        return Ok(veterinarios);
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
    [HttpGet("{veterinarianId}")]
    public async Task<IActionResult> GetVeterinarian(Guid veterinarianId)
    {
        var veterinario = await _obterVeterinarioPorIdUseCase.Run(veterinarianId);
        if (veterinario == null) return NotFound(new { mensagem = "Veterinário não encontrado." });

        return Ok(veterinario);
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