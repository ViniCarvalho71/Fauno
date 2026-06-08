using Fauno.Agenda.Application.DTOs;
using Fauno.Agenda.Application.UseCases;
using Fauno.Agenda.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;

namespace Fauno.Agenda.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
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
        public async Task<IActionResult> GetByVeterinarian([FromQuery] DateOnly? date)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var result = await _getByVetUseCase.Run(date, userId);
                return Ok(result);
            }
            catch (DomainException ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("owner")]
        public async Task<IActionResult> GetByOwner([FromQuery] DateOnly? date)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var result = await _getByOwnerUseCase.Run(date, userId);
                return Ok(result);
            }
            catch (DomainException ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        public async Task<IActionResult> MakeAppointment(AppointmentDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                await _makeAppointmentUseCase.Run(dto, userId);
                return Created();
            }
            catch (DomainException ex) { return BadRequest(ex.Message); }
        }

        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                await _cancelAppointmentUseCase.Run(id, userId);
                return NoContent();
            }
            catch (DomainException ex) { return BadRequest(ex.Message); }
        }

        [HttpPatch("{id}/confirm")]
        public async Task<IActionResult> Confirm(Guid id)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                await _confirmAppointmentUseCase.Run(id, userId);
                return NoContent();
            }
            catch (DomainException ex) { return BadRequest(ex.Message); }
        }

        [HttpPatch("{id}/finish")]
        public async Task<IActionResult> Finish(Guid id)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                await _finishAppointmentUseCase.Run(id, userId);
                return NoContent();
            }
            catch (DomainException ex) { return BadRequest(ex.Message); }
        }
    }
}
