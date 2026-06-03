using Fauno.Agenda.Application.DTOs;
using Fauno.Agenda.Application.UseCases;
using Fauno.Agenda.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
[Authorize]
public class AvailabilityController : ControllerBase
{
    private readonly GetAvailableSlotsUseCase _getAvailableSlotsUseCase;

    public AvailabilityController(GetAvailableSlotsUseCase getAvailableSlotsUseCase)
    {
        _getAvailableSlotsUseCase = getAvailableSlotsUseCase;
    }

    [HttpGet("slots")]
    public async Task<IActionResult> GetAvailableSlots([FromQuery] GetAvailableSlotsDto dto)
    {
        try
        {
            var slots = await _getAvailableSlotsUseCase.Run(dto);
            return Ok(slots);
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}