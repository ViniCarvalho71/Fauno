using Fauno.Agenda.Application.DTOs;
using Fauno.Agenda.Application.Interfaces.Http;
using Fauno.Agenda.Domain.Entities;
using Fauno.Agenda.Domain.Enums;
using Fauno.Agenda.Domain.Exceptions;
using Fauno.Agenda.Domain.Interfaces.Repositories;

namespace Fauno.Agenda.Application.UseCases
{
    public class GetAvailableSlotsUseCase
    {
        private readonly IRegisterGateway _registerGateway;
        private readonly IAvailabilityRuleRepository _availabilityRuleRepository;
        private readonly IAvailabilityExceptionRepository _availabilityExceptionRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public GetAvailableSlotsUseCase(
            IRegisterGateway registerGateway,
            IAvailabilityRuleRepository availabilityRuleRepository,
            IAvailabilityExceptionRepository availabilityExceptionRepository,
            IAppointmentRepository appointmentRepository)
        {
            _registerGateway = registerGateway;
            _availabilityRuleRepository = availabilityRuleRepository;
            _availabilityExceptionRepository = availabilityExceptionRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<IEnumerable<AvailableSlotDto>> Run(GetAvailableSlotsDto dto)
        {
            bool vetExists = await _registerGateway.VeterinarianExists(dto.VeterinarianId);
            if (!vetExists)
                throw new DomainException("Usuário inválido.");

            bool isDayBlocked = await _availabilityExceptionRepository
                .ExistsForDateAsync(dto.VeterinarianId, dto.Date);

            if (isDayBlocked)
                return [];

            var rules = await _availabilityRuleRepository
                .GetByVeterinarianIdAsync(dto.VeterinarianId);

            var specificRules = rules.Where(r => r.Recurrence.Mode == RecurrenceMode.SpecificDates && r.Recurrence.ResolveDates().Contains(dto.Date)).ToList();

            IEnumerable<AvailabilityRule> rulesToUse;

            if (specificRules.Any())
            {
                
                rulesToUse = specificRules;
            }
            else
            {
                rulesToUse = rules.Where(r =>
                    r.Recurrence.Mode == RecurrenceMode.Weekly);
            }

            var allSlots = rulesToUse
                .SelectMany(rule => rule.GenerateSlotsFor(dto.Date))
                .Distinct()
                .OrderBy(s => s.Start)
                .ToList();

            if (!allSlots.Any())
                return [];

            var existingAppointments = await _appointmentRepository
                .GetByVeterinarianAndDateAsync(dto.VeterinarianId, dto.Date);

            var availableSlots = allSlots
                .Where(slot => !existingAppointments.Any(a =>
                    TimeOnly.FromDateTime(a.Start) < slot.End &&
                    TimeOnly.FromDateTime(a.End) > slot.Start))
                .Select(slot => new AvailableSlotDto
                {
                    Date = dto.Date,
                    Start = slot.Start,
                    End = slot.End
                });

            return availableSlots;
        }
    }
}