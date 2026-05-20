using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Agenda.Application.DTOs
{
    public class AppointmentDto
    {
        public string? Title { get; private set; }
        public string? Description { get; private set; }
        public Guid OwnerId { get; private set; }
        public Guid PetId { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

    }
}
