using Fauno.Agenda.Application.Interfaces.Http;
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
        private readonly IRegisterGateway _registerGateway;

        public RemoveAvailabilityRuleUseCase(IAvailabilityRuleRepository availabilityRuleRepository, IRegisterGateway registerGateway)
        {
            _availabilityRuleRepository = availabilityRuleRepository;
            _registerGateway = registerGateway;
        }

        public async Task Run(Guid ruleId, Guid userId)
        {
            Guid VeterinarianId = await _registerGateway.GetVeterinarianIdByUserIdAsync(userId);
            if (VeterinarianId == Guid.Empty)
                throw new DomainException("Usuário inválido.");
            bool isOwner = await _availabilityRuleRepository.RuleOwnerAsync(ruleId, VeterinarianId);
            if (!isOwner)
                throw new DomainException("Regra de disponibilidade não pertence ao veterinário.");
            var rule = await _availabilityRuleRepository.GetByIdAsync(ruleId);

            if (rule is null)
                throw new DomainException("Regra de disponibilidade não encontrada.");

            rule.Remove();
            await _availabilityRuleRepository.UpdateAsync(rule);
        }
    }
}
