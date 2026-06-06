using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Agenda.Application.DTOs
{
    public class CreateAvailabilityExceptionDto
    {
        public DateOnly Date { get; set; }
        public string? Reason { get; set; }
    }

}
