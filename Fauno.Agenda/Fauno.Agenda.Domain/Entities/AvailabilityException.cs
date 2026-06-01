namespace Fauno.Agenda.Domain.Entities
{
    public class AvailabilityException : EntityBase
    {
        public Guid VeterinarianId { get; private set; }
        public DateOnly Date { get; private set; }
        public string? Reason { get; private set; }

        protected AvailabilityException() { }

        public AvailabilityException(Guid veterinarianId, DateOnly date, string? reason = null)
        {
            VeterinarianId = veterinarianId;
            Date = date;
            Reason = reason;
        }
    }
}