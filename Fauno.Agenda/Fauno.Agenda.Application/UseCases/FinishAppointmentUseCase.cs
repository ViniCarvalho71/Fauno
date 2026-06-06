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

        public async Task Run(Guid appointmentId, Guid userId)
        {
            //Guid VeterinarianId = await _appointmentRepository.GetVeterinarianIdByUserIdAsync(userId);
            Guid VeterinarianId = userId;
            if (VeterinarianId == Guid.Empty)
                throw new DomainException("Apenas o veterinário pode cancelar.");
            var appointment = await _appointmentRepository.GetByIdAndVeterinarianIdAsync(appointmentId, VeterinarianId);
            if (appointment is null)
                throw new DomainException("Consulta não encontrada.");

            appointment.Finish();
            await _appointmentRepository.UpdateAsync(appointment);
        }
    }
}
