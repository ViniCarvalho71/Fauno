using Microsoft.AspNetCore.Mvc;

namespace Fauno.Agenda.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgendaController : ControllerBase
    {
        
        [HttpGet("")]
        public IActionResult Get()
        {

            var authHeader = Request.Headers["Authorization"].ToString();
            var token = authHeader.Replace("Bearer ", "");
            
            return Ok();
        }
    }
}
