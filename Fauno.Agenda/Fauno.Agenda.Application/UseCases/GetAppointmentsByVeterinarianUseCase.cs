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

        public async Task<IEnumerable<AppointmentResponseDto>> Run(GetAppointmentsByVeterinarianDto dto)
        {
            bool vetExists = await _registerGateway.VeterinarianExists(dto.VeterinarianId);
            if (!vetExists)
                throw new DomainException("Veterinário inválido.");

            var appointments = dto.Date.HasValue
                ? await _appointmentRepository.GetByVeterinarianAndDateAsync(dto.VeterinarianId, dto.Date.Value)
                : await _appointmentRepository.GetByVeterinarianIdAsync(dto.VeterinarianId);

            return appointments.Select(ToDto);
        }

        private static AppointmentResponseDto ToDto(Appointment a) => new()
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
        };
    }
}
