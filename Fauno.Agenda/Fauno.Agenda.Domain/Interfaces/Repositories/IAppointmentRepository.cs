using Fauno.Agenda.Domain.Entities;

namespace Fauno.Agenda.Domain.Interfaces.Repositories
{
    public interface IAppointmentRepository
    {
        Task AddAsync(Appointment appointment);
        Task UpdateAsync(Appointment appointment);
        Task<Appointment?> GetByIdAndVeterinarianIdAsync(Guid appointmentId, Guid veterinarianId);

        Task<IEnumerable<Appointment>> GetByVeterinarianIdAsync(Guid veterinarianId);
        Task<IEnumerable<Appointment>> GetByOwnerIdAsync(Guid ownerId);
        Task<IEnumerable<Appointment>> GetByVeterinarianAndDateAsync(Guid veterinarianId, DateOnly date);
        Task<IEnumerable<Appointment>> GetByOwnerAndDateAsync(Guid ownerId, DateOnly date);

        Task<bool> HasConflictAsync(Guid veterinarianId, DateTime start, DateTime end);
    }
}