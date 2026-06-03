using Fauno.Agenda.Application.DTOs;
using Fauno.Agenda.Application.UseCases;
using Fauno.Agenda.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fauno.Agenda.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AvailabilityRuleController : ControllerBase
    {
        private readonly CreateAvailabilityRuleUseCase _createAvailabilityRuleUseCase;
        private readonly RemoveAvailabilityRuleUseCase _removeUseCase;

        public AvailabilityRuleController(
            CreateAvailabilityRuleUseCase createAvailabilityRuleUseCase,
            RemoveAvailabilityRuleUseCase removeUseCase)
        {
            _createAvailabilityRuleUseCase = createAvailabilityRuleUseCase;
            _removeUseCase = removeUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAvailabilityRuleDto dto)
        {
            try
            {
                await _createAvailabilityRuleUseCase.Run(dto);
                return Created();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try
            {
                await _removeUseCase.Run(id);
                return NoContent();
            }
            catch (DomainException ex) { return BadRequest(ex.Message); }
        }
    }
}
