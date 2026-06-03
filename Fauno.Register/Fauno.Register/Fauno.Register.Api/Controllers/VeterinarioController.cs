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
}