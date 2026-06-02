using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fauno.Register.Application.UseCases;

namespace Fauno.Register.Api.Controllers;

[ApiController]
[Route("api/cadastros/donos")]
public class DonoController : ControllerBase
{
    private readonly CadastrarDonoUseCase _useCase;

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
}