using Fauno.Agenda.Domain.Entities;

namespace Fauno.Agenda.Domain.Interfaces.Repositories
{
    public interface IAppointmentRepository
    {
        Task AddAsync(Appointment appointment);
        Task UpdateAsync(Appointment appointment);
        Task<Appointment?> GetByIdAsync(Guid id);
        Task<IEnumerable<Appointment>> GetByVeterinarianIdAsync(Guid veterinarianId);
        Task<IEnumerable<Appointment>> GetByOwnerIdAsync(Guid ownerId);
        Task<IEnumerable<Appointment>> GetByVeterinarianAndDateAsync(Guid veterinarianId, DateOnly date);
        Task<bool> HasConflictAsync(Guid veterinarianId, DateTime start, DateTime end);
    }
}