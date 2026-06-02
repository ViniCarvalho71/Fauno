using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Agenda.Application.DTOs
{
    public class GetAvailableSlotsDto
    {
        public Guid VeterinarianId { get; set; }
        public DateOnly Date { get; set; }
    }
}
