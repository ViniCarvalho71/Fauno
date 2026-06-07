using Fauno.Agenda.Application.Interfaces.Http;
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
        private readonly IRegisterGateway _registerGateway;


        public FinishAppointmentUseCase(IAppointmentRepository appointmentRepository, IRegisterGateway registerGateway)
        {
            _appointmentRepository = appointmentRepository;
            _registerGateway = registerGateway;
        }

        public async Task Run(Guid appointmentId, Guid userId)
        {
            Guid VeterinarianId = await _registerGateway.GetVeterinarianIdByUserIdAsync(userId);
            if (VeterinarianId == Guid.Empty)
                throw new DomainException("Usuário inválido.");
            var appointment = await _appointmentRepository.GetByIdAndVeterinarianIdAsync(appointmentId, VeterinarianId);
            if (appointment is null)
                throw new DomainException("Consulta não encontrada.");

            appointment.Finish();
            await _appointmentRepository.UpdateAsync(appointment);
        }
    }
}
