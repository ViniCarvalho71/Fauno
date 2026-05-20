//using Fauno.Agenda.Domain.Enums;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Fauno.Agenda.Domain.Entities
//{
//    public class Schedule : EntityBase
//    {
//        public Guid VetenerianId { get; private set; }
//        public DateTime Start { get; private set; }
//        public DateTime End { get; private set; }

//        public bool IsAvailable =>
//            Appointment is null ||
            
//            Appointment.Status == AppointmentStatus.Cancelled;
//        public Appointment? Appointment { get; private set; }
//        protected Schedule() { }
//        public Schedule(Guid veterianId, DateTime start, DateTime end)
//        {
//            if (end < start) 
//                throw new Exception("Horário inválido");
            
//            VetenerianId = veterianId;
//            Start = start;
//            End = end;
//        }
//        public void Reserve(Guid ownerId, Guid petId, string? title = null, string? description = null)
//        {
//            if (!IsAvailable)
//                throw new Exception("Horário não disponível");
//            Appointment = new Appointment(description, title, ownerId, petId, this);
            
//        }
//        public void CancelAppointment()
//        {
//            if (IsAvailable)
//                throw new Exception("Horário já disponível");
//            Appointment.Cancel();
//            Appointment = null;
//        }
//        public void CancelSchedule()
//        {
//            if (!IsAvailable)
//                throw new Exception("Não é possível cancelar um horário reservado");
//            Remove();
//        }

        
//    }
//}
