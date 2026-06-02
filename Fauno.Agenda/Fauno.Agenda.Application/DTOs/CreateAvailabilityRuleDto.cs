using Fauno.Agenda.Domain.Enums;

namespace Fauno.Agenda.Application.DTOs
{
    
        public class CreateAvailabilityRuleDto
        {
            public Guid VeterinarianId { get; set; }
            public TimeOnly SlotStart { get; set; }
            public TimeOnly SlotEnd { get; set; }
            public int SlotDurationMinutes { get; set; }
            public PauseWindowDto? Pause { get; set; }
            public RecurrenceDto Recurrence { get; set; } = null!;
        }

        
    
}
