using Fauno.Agenda.Application.DTOs;
using Fauno.Agenda.Application.Interfaces.Http;
using Fauno.Agenda.Domain.Exceptions;
using Fauno.Agenda.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Agenda.Application.UseCases
{
    public class GetAppointmentsByOwnerUseCase
    {
        private readonly IRegisterGateway _registerGateway;
        private readonly IAppointmentRepository _appointmentRepository;

        public GetAppointmentsByOwnerUseCase(
            IRegisterGateway registerGateway,
            IAppointmentRepository appointmentRepository)
        {
            _registerGateway = registerGateway;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<IEnumerable<AppointmentResponseDto>> Run(Guid ownerId)
        {
            bool ownerExists = await _registerGateway.OwnerExists(ownerId);
            if (!ownerExists)
                throw new DomainException("Dono de pet inválido.");

            var appointments = await _appointmentRepository.GetByOwnerIdAsync(ownerId);

            return appointments.Select(a => new AppointmentResponseDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Status = a.Status.ToString(),
                AppointmentType = a.AppointmentType.ToString(),
                VeterinarianId = a.VeterinarianId,
                OwnerId = a.OwnerId,
                PetId = a.PetId,
                Start = a.Start,
                End = a.End,
                CreatedAt = a.CreatedAt
            });
        }
    }
}
