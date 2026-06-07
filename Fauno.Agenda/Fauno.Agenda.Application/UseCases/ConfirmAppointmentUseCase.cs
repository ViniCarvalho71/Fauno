using Fauno.Agenda.Application.Interfaces.Http;
using Fauno.Agenda.Domain.Exceptions;
using Fauno.Agenda.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Agenda.Application.UseCases
{
    public class ConfirmAppointmentUseCase
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IRegisterGateway _registerGateway;


        public ConfirmAppointmentUseCase(IAppointmentRepository appointmentRepository, IRegisterGateway registerGateway)
        {
            _appointmentRepository = appointmentRepository;
            _registerGateway = registerGateway;
        }

        public async Task Run(Guid appointmentId, Guid userId)
        {
            Guid VeterinarianId = await _registerGateway.GetVeterinarianIdByUserIdAsync(userId);
            if (VeterinarianId == Guid.Empty)
                throw new DomainException("Apenas o veterinário pode cancelar.");
            var appointment = await _appointmentRepository.GetByIdAndVeterinarianIdAsync(appointmentId, VeterinarianId);
            if (appointment is null)
                throw new DomainException("Consulta não encontrada.");

            appointment.Confirm();
            await _appointmentRepository.UpdateAsync(appointment);
        }
    }
}
