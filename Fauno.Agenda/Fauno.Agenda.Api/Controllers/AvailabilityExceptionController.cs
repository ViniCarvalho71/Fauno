using Fauno.Agenda.Application.DTOs;
using Fauno.Agenda.Application.UseCases;
using Fauno.Agenda.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Fauno.Agenda.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AvailabilityExceptionController : ControllerBase
    {
        private readonly CreateAvailabilityExceptionUseCase _createExceptionUseCase;

        public AvailabilityExceptionController(CreateAvailabilityExceptionUseCase createExceptionUseCase)
        {
            _createExceptionUseCase = createExceptionUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAvailabilityExceptionDto dto)
        {
            try
            {
                await _createExceptionUseCase.Run(dto);
                return Created();
            }
            catch (DomainException ex) { return BadRequest(ex.Message); }
        }
    }
}
