using Fauno.Agenda.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Agenda.Application.Interfaces.Repositories
{
    public interface IAppointmentRepository
    {
        void Add(Appointment appointment);
        bool VerifyTimeRange(DateTime start, DateTime end);
    }
}
