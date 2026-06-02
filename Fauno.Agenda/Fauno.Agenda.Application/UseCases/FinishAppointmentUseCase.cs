using Fauno.Agenda.Domain.Exceptions;
using Fauno.Agenda.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Agenda.Application.UseCases
{
    public class FinishAppointmentUseCase
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public FinishAppointmentUseCase(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task Run(Guid appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment is null)
                throw new DomainException("Consulta não encontrada.");

            appointment.Finish();
            await _appointmentRepository.UpdateAsync(appointment);
        }
    }
}
