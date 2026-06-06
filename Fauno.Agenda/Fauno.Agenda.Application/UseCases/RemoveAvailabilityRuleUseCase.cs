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

        public async Task Run(Guid ruleId, Guid userId)
        {
            //Guid VetenerianId = await _registerGateway.GetVeterinarianIdByUserId(userId);
            //if (VetenerianId == Guid.Empty)
            //    throw new DomainException("Usuário não é um veterinário.");
            Guid VeterinarianId = userId; // Troca isso oboviamente
            //bool isOwner = await _availabilityRuleRepository.RuleOwnerAsync(ruleId, VeterinarianId);
            //if(!isOwner)
            //    throw new DomainException("Regra de disponibilidade não pertence ao veterinário.");
            var rule = await _availabilityRuleRepository.GetByIdAsync(ruleId);

            if (rule is null)
                throw new DomainException("Regra de disponibilidade não encontrada.");

            rule.Remove();
            await _availabilityRuleRepository.UpdateAsync(rule);
        }
    }
}
