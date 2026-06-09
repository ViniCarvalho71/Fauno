using Fauno.Agenda.Application.DTOs;
using Fauno.Agenda.Application.Interfaces.Http;
using Fauno.Agenda.Domain.Entities;
using Fauno.Agenda.Domain.Exceptions;
using Fauno.Agenda.Domain.Interfaces.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
namespace Fauno.Agenda.Application.UseCases
{
    public class MakeAppointmentUseCase
    {
        private readonly IRegisterGateway _registerGateway;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IAvailabilityRuleRepository _availabilityRuleRepository;
        private readonly IAvailabilityExceptionRepository _availabilityExceptionRepository;


        public MakeAppointmentUseCase(IRegisterGateway registerGateway,
            IAppointmentRepository appointmentRepository,
            IAvailabilityRuleRepository availabilityRuleRepository,
            IAvailabilityExceptionRepository availabilityExceptionRepository){ 
            _registerGateway = registerGateway;
            _appointmentRepository = appointmentRepository;
            _availabilityRuleRepository = availabilityRuleRepository;
            _availabilityExceptionRepository = availabilityExceptionRepository;

        }


        public async Task Run(AppointmentDto appointmentDto, Guid userId)
        {
            Guid ownerId = await _registerGateway.GetOwnerIdByUserIdAsync(userId);
            if (ownerId == Guid.Empty)
                throw new DomainException("Usuário inválido.");
            var veterinarianName = await _registerGateway.GetVeterinarianNameById(appointmentDto.VeterinarianId);

            var ownerName = await _registerGateway.GetOwnerNameById(ownerId);

            var petName = await _registerGateway.GetPetNameById(appointmentDto.PetId);
            if (veterinarianName == null)
                throw new DomainException("Veterinário inválido");
            if (petName == null)
                throw new DomainException("Pet inválido");
            bool hasConflict = await _appointmentRepository.HasConflictAsync(
                appointmentDto.VeterinarianId,
                appointmentDto.Start,
                appointmentDto.End);

            if (hasConflict)
                throw new DomainException("Horário indisponível");

            var date = DateOnly.FromDateTime(appointmentDto.Start);

            bool isDayBlocked = await _availabilityExceptionRepository
                .ExistsForDateAsync(appointmentDto.VeterinarianId, date);

            if (isDayBlocked)
                throw new DomainException("Veterinário indisponível nessa data.");

            var rules = await _availabilityRuleRepository
                .GetActiveForDateAsync(appointmentDto.VeterinarianId);

            var slotStart = TimeOnly.FromDateTime(appointmentDto.Start);
            var slotEnd = TimeOnly.FromDateTime(appointmentDto.End);

            bool slotExists = rules
                .SelectMany(r => r.GenerateSlotsFor(date))
                .Any(s => s.Start == slotStart && s.End == slotEnd);

            if (!slotExists)
                throw new DomainException("O horário escolhido não está disponível na agenda do veterinário.");

            var newAppointment = new Appointment(
                appointmentDto.Description,
                appointmentDto.Title,
                appointmentDto.VeterinarianId,
                ownerId,
                appointmentDto.PetId,
                appointmentDto.Start,
                appointmentDto.End,
                appointmentDto.AppointmentType,
                veterinarianName,
                ownerName,
                petName);

            await _appointmentRepository.AddAsync(newAppointment);
        }

    }
}
