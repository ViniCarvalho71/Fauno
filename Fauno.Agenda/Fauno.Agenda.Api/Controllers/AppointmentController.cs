using Microsoft.AspNetCore.Mvc;

namespace Fauno.Agenda.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentController : ControllerBase
    {
      
        [HttpPost()]
        public IActionResult MakeAppointment()
        {
            return Ok();
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
