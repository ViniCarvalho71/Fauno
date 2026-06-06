using Fauno.Agenda.Application.DTOs;
using Fauno.Agenda.Application.Interfaces.Http;
using Fauno.Agenda.Domain.Exceptions;
using Fauno.Agenda.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Agenda.Application.UseCases
{
    public class GetAppointmentsByVeterinarianUseCase
    {
        private readonly IRegisterGateway _registerGateway;
        private readonly IAppointmentRepository _appointmentRepository;

        public GetAppointmentsByVeterinarianUseCase(
            IRegisterGateway registerGateway,
            IAppointmentRepository appointmentRepository)
        {
            _registerGateway = registerGateway;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<IEnumerable<AppointmentResponseDto>> Run(DateOnly? date, Guid userId)
        {

            //Guid VetenerianId = await _registerGateway.GetVeterinarianIdByUserId(userId);
            //if (VetenerianId == Guid.Empty)
            //    throw new DomainException("Usuário não é um veterinário.");
            Guid VeterinarianId = userId; // Tira isso pelor né
            bool vetExists = await _registerGateway.VeterinarianExists(VeterinarianId);
            if (!vetExists)
                throw new DomainException("Veterinário inválido.");

            var appointments = date.HasValue
                ? await _appointmentRepository.GetByVeterinarianAndDateAsync(VeterinarianId, date.Value)
                : await _appointmentRepository.GetByVeterinarianIdAsync(VeterinarianId);
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
