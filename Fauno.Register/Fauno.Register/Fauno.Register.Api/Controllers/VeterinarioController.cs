using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fauno.Register.Application.UseCases;
using Fauno.Register.Application.DTOs.Requests;

namespace Fauno.Register.Api.Controllers;

[ApiController]
[Route("api/cadastros/veterinarios")]
public class VeterinarioController : ControllerBase
{
    private readonly CadastrarVeterinarioUseCase _useCase;
    private readonly ObterVeterinarioIdPorUserIdUseCase _obterVetIdUseCase;
    private readonly VerificarVeterinarioExisteUseCase _verificarVetExisteUseCase;

    public VeterinarioController(CadastrarVeterinarioUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] CadastrarVeterinarioRequest request)
    {
        try
        {
            var vet = await _useCase.Run(request);
            return Created($"/api/cadastros/veterinarios/{vet.Id}", new { vet.Id, vet.Nome, Cpf = vet.Cpf.Numero });
        }
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }
    
    [HttpGet("usuario/{userId}/id")]
    public async Task<IActionResult> GetVeterinarianIdByUserId(Guid userId)
    {
        var id = await _obterVetIdUseCase.Run(userId);
        if (id == null) return NotFound(new { mensagem = "Veterinário não encontrado para este usuário." });
    
        return Ok(new { veterinarianId = id });
    }

    [HttpGet("usuario/{userId}/existe")]
    public async Task<IActionResult> VeterinarianExists(Guid id)
    {
        var existe = await _verificarVetExisteUseCase.Run(id);
        return Ok(existe);
    }
}