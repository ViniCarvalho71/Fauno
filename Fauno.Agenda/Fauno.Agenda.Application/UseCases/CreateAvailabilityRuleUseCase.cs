using Fauno.Agenda.Application.DTOs;
using Fauno.Agenda.Application.Interfaces.Http;
using Fauno.Agenda.Domain.Entities;
using Fauno.Agenda.Domain.Enums;
using Fauno.Agenda.Domain.Exceptions;
using Fauno.Agenda.Domain.Interfaces.Repositories;
using Fauno.Agenda.Domain.ValueObjects;

namespace Fauno.Agenda.Application.UseCases
{
    public class CreateAvailabilityRuleUseCase
    {
        private readonly IRegisterGateway _registerGateway;
        private readonly IAvailabilityRuleRepository _availabilityRuleRepository;

        public CreateAvailabilityRuleUseCase(
            IRegisterGateway registerGateway,
            IAvailabilityRuleRepository availabilityRuleRepository)
        {
            _registerGateway = registerGateway;
            _availabilityRuleRepository = availabilityRuleRepository;
        }

        public async Task Run(CreateAvailabilityRuleDto dto)
        {
            bool vetExists = await _registerGateway.VeterinarianExists(dto.VeterinarianId);
            if (!vetExists)
                throw new DomainException("Veterinário inválido.");

            Recurrence recurrence = dto.Recurrence.Mode == RecurrenceMode.Weekly
                ? Recurrence.Weekly(
                    dto.Recurrence.DaysOfWeek!,
                    dto.Recurrence.PeriodStart!.Value,
                    dto.Recurrence.PeriodEnd!.Value)
                : Recurrence.SpecificDates(dto.Recurrence.Dates!);

            PauseWindow? pause = dto.Pause is not null
                ? new PauseWindow(dto.Pause.Start, dto.Pause.End)
                : null;

            var rule = new AvailabilityRule(
                dto.VeterinarianId,
                dto.SlotStart,
                dto.SlotEnd,
                dto.SlotDurationMinutes,
                recurrence,
                pause);

            await _availabilityRuleRepository.AddAsync(rule);
        }
    }
}