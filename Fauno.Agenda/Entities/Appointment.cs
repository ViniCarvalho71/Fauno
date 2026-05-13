using Fauno.Agenda.Api.Enums;

namespace Fauno.Agenda.Api.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; } 
        public Guid OwnerId { get; set; }
        public Guid PetId { get; set; }
        public Guid VeterinarianId { get; set; }
    }
}
