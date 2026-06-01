using Fauno.Agenda.Domain.Entities;

namespace Fauno.Agenda.Domain.Interfaces.Repositories
{
    public interface IAppointmentRepository
    {
        Task AddAsync(Appointment appointment);
        Task<Appointment?> GetByIdAsync(Guid id);
        Task<IEnumerable<Appointment>> GetByVeterinarianIdAsync(Guid veterinarianId);
        Task<bool> HasConflictAsync(Guid veterinarianId, DateTime start, DateTime end);
    }
}