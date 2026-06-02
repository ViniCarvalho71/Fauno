using Fauno.Agenda.Domain.Entities;
using Fauno.Agenda.Domain.Enums;
using Fauno.Agenda.Domain.Exceptions;

public class Appointment : EntityBase
{
    public string? Title { get; private set; }
    public string? Description { get; private set; }
    public AppointmentStatus Status { get; private set; }
    public AppointmentType AppointmentType { get; private set; }
    public Guid VeterinarianId { get; private set; }
    public Guid OwnerId { get; private set; }
    public Guid PetId { get; private set; }
    public DateTime Start { get; private set; }
    public DateTime End { get; private set; }

    protected Appointment() { }

    public Appointment(
        string? description,
        string? title,
        Guid veterinarianId,
        Guid ownerId,
        Guid petId,
        DateTime start,
        DateTime end)
    {
        Title = title ?? "Consulta";
        Description = description ?? "Sem descrição";
        Status = AppointmentStatus.Scheduled;
        VeterinarianId = veterinarianId;
        OwnerId = ownerId;
        PetId = petId;
        Start = start;
        End = end;
    }

    public void Cancel()
    {
        if (Status == AppointmentStatus.Cancelled)
            throw new DomainException("Consulta já cancelada.");
        if (Status == AppointmentStatus.Finished)
            throw new DomainException("Consulta já finalizada.");
        Status = AppointmentStatus.Cancelled;
    }

    public void Confirm()
    {
        if (Status != AppointmentStatus.Scheduled)
            throw new DomainException("Apenas consultas agendadas podem ser confirmadas.");
        Status = AppointmentStatus.Confirmed;
    }

    public void Finish()
    {
        if (Status == AppointmentStatus.Cancelled)
            throw new DomainException("Não é possível finalizar uma consulta cancelada.");
        if (Status == AppointmentStatus.Finished)
            throw new DomainException("Consulta já finalizada.");
        Status = AppointmentStatus.Finished;
    }
}