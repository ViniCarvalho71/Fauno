using Fauno.Agenda.Application.DTOs;
using Fauno.Agenda.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Fauno.Agenda.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly MakeAppointmentUseCase _makeAppointmentUseCase;
        public AppointmentController(MakeAppointmentUseCase makeAppointmentUseCase)
        { 
            _makeAppointmentUseCase = makeAppointmentUseCase;
        }
      
        [HttpPost()]
        public IActionResult MakeAppointment(AppointmentDto requestDto)
        {
            try
            {
                _makeAppointmentUseCase.RunAsync(requestDto);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet()]
        public IActionResult CancelAppointment()
        {
            return Ok();
        }

        //[HttpGet]
        //public IActionResult 

    }
}
