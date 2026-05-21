using Fauno.Agenda.Application.Interfaces.Repositories;
using Fauno.Agenda.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Agenda.Infrastructure.Repositories
{
    internal class AppointmentRepository : IAppointmentRepository
    {
        public void Add(Appointment appointment)
        {
            // Aqui é para adicionar um registro ao banco
            throw new NotImplementedException();
        }

        public bool VerifyTimeRange(DateTime start, DateTime end)
        {
            // Aqui você pode implementar a lógica para verificar se o intervalo de tempo está disponível. basicamente é verificar dentro de um when se existe algum agendamento que se sobreponha com o intervalo fornecido.
            throw new NotImplementedException();
        }
    }
}
