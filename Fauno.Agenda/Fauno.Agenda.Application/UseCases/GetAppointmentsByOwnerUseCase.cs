using Fauno.Agenda.Application.DTOs;
using Fauno.Agenda.Application.Interfaces.Http;
using Fauno.Agenda.Domain.Exceptions;
using Fauno.Agenda.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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

        public async Task<IEnumerable<AppointmentResponseDto>> Run(DateOnly? date, Guid userId)
        {
            Guid OwnerId = await _registerGateway.GetOwnerIdByUserIdAsync(userId);
            if (OwnerId == Guid.Empty)
                throw new DomainException("Usuário inválido");
            

            var appointments = date.HasValue
                ? await _appointmentRepository.GetByOwnerAndDateAsync(OwnerId, date.Value)
                : await _appointmentRepository.GetByOwnerIdAsync(OwnerId);

            return appointments.Select(a => new AppointmentResponseDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Status = a.Status.ToString(),
                AppointmentType = a.AppointmentType.ToString(),
                VeterinarianId = a.VeterinarianId,
                VeterinarianName = a.VeterinarianName,
                OwnerId = a.OwnerId,
                OwnerName= a.OwnerName,
                PetId = a.PetId,
                PetName= a.PetName,
                Start = a.Start,
                End = a.End,
                CreatedAt = a.CreatedAt
            });
        }
    }
}
