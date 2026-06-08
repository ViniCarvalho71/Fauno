using Fauno.Agenda.Domain.Entities;

namespace Fauno.Agenda.Domain.Interfaces.Repositories
{
    public interface IAvailabilityExceptionRepository
    {
        Task AddAsync(AvailabilityException exception);
        Task<IEnumerable<AvailabilityException>> GetByVeterinarianIdAsync(Guid veterinarianId);
        Task<bool> ExistsForDateAsync(Guid veterinarianId, DateOnly date);
    }
}