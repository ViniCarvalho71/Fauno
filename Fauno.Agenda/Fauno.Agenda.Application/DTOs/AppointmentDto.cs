using Fauno.Agenda.Domain.Enums;

public class AppointmentDto
{
    public Guid VeterinarianId { get; set; }
    public Guid OwnerId { get; set; }
    public Guid PetId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public AppointmentType AppointmentType { get; set; }
}