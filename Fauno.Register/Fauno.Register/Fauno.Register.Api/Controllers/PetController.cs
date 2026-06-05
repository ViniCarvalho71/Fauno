using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fauno.Register.Application.DTOs;
using Fauno.Register.Application.UseCases;

namespace Fauno.Register.Api.Controllers;

[ApiController]
[Route("api/cadastros/pets")]
public class PetController : ControllerBase
{
    private readonly CadastrarPetUseCase _cadastrarPetUseCase;
    private readonly ListarPetsDoDonoUseCase _listarPetsDoDonoUseCase;
    private readonly AtualizarPetUseCase _atualizarPetUseCase;
    private readonly BuscarHistoricoPetUseCase _buscarHistoricoPetUseCase;

    public PetController(
        CadastrarPetUseCase cadastrarPetUseCase, 
        ListarPetsDoDonoUseCase listarPetsDoDonoUseCase,
        AtualizarPetUseCase atualizarPetUseCase,
        BuscarHistoricoPetUseCase buscarHistoricoPetUseCase)
    {
        _cadastrarPetUseCase = cadastrarPetUseCase;
        _listarPetsDoDonoUseCase = listarPetsDoDonoUseCase;
        _atualizarPetUseCase = atualizarPetUseCase;
        _buscarHistoricoPetUseCase = buscarHistoricoPetUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] CadastrarPetRequest request)
    {
        try
        {
            var pet = await _cadastrarPetUseCase.Run(request);
            return Created($"/api/cadastros/pets/{pet.Id}", pet);
        }
        catch (Exception ex) { return BadRequest(new { erro = ex.Message }); }
    }

    [HttpGet("dono/{donoId}")]
    public async Task<IActionResult> ListarPorDono(Guid donoId)
    {
        var pets = await _listarPetsDoDonoUseCase.Run(donoId);
        return Ok(pets);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarPetRequest request)
    {
        try
        {
            var pet = await _atualizarPetUseCase.Run(id, request);
            return Ok(pet);
        }
        catch (Exception ex) { return BadRequest(new { erro = ex.Message }); }
    }

    [HttpGet("{id}/historico")]
    public async Task<IActionResult> VerHistorico(Guid id)
    {
        try
        {
            var historico = await _buscarHistoricoPetUseCase.Run(id);
            return Ok(historico);
        }
        catch (Exception ex) { return BadRequest(new { erro = ex.Message }); }
    }
}