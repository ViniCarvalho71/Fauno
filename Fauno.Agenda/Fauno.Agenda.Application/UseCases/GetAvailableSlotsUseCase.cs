using Fauno.Agenda.Application.DTOs;
using Fauno.Agenda.Application.Interfaces.Http;
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
                throw new DomainException("Veterinário inválido.");

            // Dia bloqueado por exceção — retorna vazio direto
            bool isDayBlocked = await _availabilityExceptionRepository
                .ExistsForDateAsync(dto.VeterinarianId, dto.Date);

            if (isDayBlocked)
                return [];

            // Gera todos os slots possíveis para o dia pelas regras ativas
            var rules = await _availabilityRuleRepository
                .GetByVeterinarianIdAsync(dto.VeterinarianId);

            var allSlots = rules
                .SelectMany(rule => rule.GenerateSlotsFor(dto.Date))
                .Distinct()
                .OrderBy(s => s.Start)
                .ToList();

            if (!allSlots.Any())
                return [];

            // Busca appointments já existentes no dia
            var existingAppointments = await _appointmentRepository
                .GetByVeterinarianAndDateAsync(dto.VeterinarianId, dto.Date);

            // Remove slots que colidem com appointments existentes
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