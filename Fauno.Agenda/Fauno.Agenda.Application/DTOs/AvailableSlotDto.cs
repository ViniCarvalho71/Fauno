namespace Fauno.Agenda.Application.DTOs
{
    public class AvailableSlotDto
    {
        public DateOnly Date { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
    }

    
}