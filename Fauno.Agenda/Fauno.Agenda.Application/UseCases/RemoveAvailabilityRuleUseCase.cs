using Fauno.Agenda.Domain.Exceptions;
using Fauno.Agenda.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Agenda.Application.UseCases
{
    public class RemoveAvailabilityRuleUseCase
    {
        private readonly IAvailabilityRuleRepository _availabilityRuleRepository;

        public RemoveAvailabilityRuleUseCase(IAvailabilityRuleRepository availabilityRuleRepository)
        {
            _availabilityRuleRepository = availabilityRuleRepository;
        }

        public async Task Run(Guid ruleId)
        {
            var rule = await _availabilityRuleRepository.GetByIdAsync(ruleId);

            if (rule is null)
                throw new DomainException("Regra de disponibilidade não encontrada.");

            rule.Remove(); // soft delete via EntityBase
            await _availabilityRuleRepository.UpdateAsync(rule);
        }
    }
}
