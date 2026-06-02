using Fauno.Agenda.Application.DTOs;
using Fauno.Agenda.Application.UseCases;
using Fauno.Agenda.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Fauno.Agenda.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly MakeAppointmentUseCase _makeAppointmentUseCase;
        private readonly CancelAppointmentUseCase _cancelAppointmentUseCase;
        private readonly ConfirmAppointmentUseCase _confirmAppointmentUseCase;
        private readonly FinishAppointmentUseCase _finishAppointmentUseCase;
        private readonly GetAppointmentsByVeterinarianUseCase _getByVetUseCase;
        private readonly GetAppointmentsByOwnerUseCase _getByOwnerUseCase;

        public AppointmentController(
            MakeAppointmentUseCase makeAppointmentUseCase,
            CancelAppointmentUseCase cancelAppointmentUseCase,
            ConfirmAppointmentUseCase confirmAppointmentUseCase,
            FinishAppointmentUseCase finishAppointmentUseCase,
            GetAppointmentsByVeterinarianUseCase getByVetUseCase,
            GetAppointmentsByOwnerUseCase getByOwnerUseCase
            )
        {
            _makeAppointmentUseCase = makeAppointmentUseCase;
            _cancelAppointmentUseCase = cancelAppointmentUseCase;
            _confirmAppointmentUseCase = confirmAppointmentUseCase;
            _finishAppointmentUseCase = finishAppointmentUseCase;
            _getByVetUseCase = getByVetUseCase;
            _getByOwnerUseCase = getByOwnerUseCase;
        }

        [HttpGet("veterinarian")]
        public async Task<IActionResult> GetByVeterinarian([FromQuery] GetAppointmentsByVeterinarianDto dto)
        {
            try
            {
                var result = await _getByVetUseCase.Run(dto);
                return Ok(result);
            }
            catch (DomainException ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("owner/{ownerId}")]
        public async Task<IActionResult> GetByOwner(Guid ownerId)
        {
            try
            {
                var result = await _getByOwnerUseCase.Run(ownerId);
                return Ok(result);
            }
            catch (DomainException ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        public async Task<IActionResult> MakeAppointment(AppointmentDto dto)
        {
            try
            {
                await _makeAppointmentUseCase.Run(dto);
                return Created();
            }
            catch (DomainException ex) { return BadRequest(ex.Message); }
        }

        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            try
            {
                await _cancelAppointmentUseCase.Run(id);
                return NoContent();
            }
            catch (DomainException ex) { return BadRequest(ex.Message); }
        }

        [HttpPatch("{id}/confirm")]
        public async Task<IActionResult> Confirm(Guid id)
        {
            try
            {
                await _confirmAppointmentUseCase.Run(id);
                return NoContent();
            }
            catch (DomainException ex) { return BadRequest(ex.Message); }
        }

        [HttpPatch("{id}/finish")]
        public async Task<IActionResult> Finish(Guid id)
        {
            try
            {
                await _finishAppointmentUseCase.Run(id);
                return NoContent();
            }
            catch (DomainException ex) { return BadRequest(ex.Message); }
        }
    }
}
