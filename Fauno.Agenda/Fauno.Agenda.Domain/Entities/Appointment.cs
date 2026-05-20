using Fauno.Agenda.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Agenda.Domain.Entities
{
    public class Appointment : EntityBase
    {
        public string? Title { get; private set; }
        public string? Description { get; private set; }
        public AppointmentStatus Status { get; private set; }
        public  Guid OwnerId { get; private set; }
        public Guid PetId { get; private set; }
        public DateTime Start {  get; private set; }
        public DateTime End { get; private set; }

        protected Appointment()
        {

        }
        public Appointment(string? description, string? title, Guid ownerId, Guid petId, DateTime start, DateTime end)
        {
            Title = title;
            Description = description;
            Status = AppointmentStatus.Scheduled;
            OwnerId = ownerId;
            PetId = petId;
            Start = start;
            End = end;
            if (Title == null)
                Title = $"Consulta";
            if (Description == null)
                Description = "Sem descrição";
        }

        public void Cancel()
        {
            if (Status == AppointmentStatus.Cancelled)
                throw new Exception("Consulta já cancelada");
            if (Status == AppointmentStatus.Finished)
                throw new Exception("Consulta já finalizada");
            Status = AppointmentStatus.Cancelled;
        }
    }
}
