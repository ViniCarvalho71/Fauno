using Fauno.Agenda.Domain.Entities;

namespace Fauno.Agenda.Domain.Interfaces.Repositories
{
    public interface IAvailabilityRuleRepository
    {
        Task AddAsync(AvailabilityRule rule);
        Task UpdateAsync(AvailabilityRule rule);
        Task<AvailabilityRule?> GetByIdAsync(Guid id);
        Task<IEnumerable<AvailabilityRule>> GetByVeterinarianIdAsync(Guid veterinarianId);
        Task<IEnumerable<AvailabilityRule>> GetActiveForDateAsync(Guid veterinarianId);
        Task<bool> RuleOwnerAsync(Guid ruleId, Guid veterinarianId);
    }
}