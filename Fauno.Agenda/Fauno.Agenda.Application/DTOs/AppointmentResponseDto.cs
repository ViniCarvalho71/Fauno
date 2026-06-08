using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Agenda.Application.DTOs
{
    public class AppointmentResponseDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = null!;
        public string AppointmentType { get; set; } = null!;
        public Guid VeterinarianId { get; set; }
        public Guid OwnerId { get; set; }
        public Guid PetId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
