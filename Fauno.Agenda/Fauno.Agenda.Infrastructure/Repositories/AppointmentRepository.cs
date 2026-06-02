using Fauno.Agenda.Domain.Entities;
using Fauno.Agenda.Domain.Enums;
using Fauno.Agenda.Domain.Interfaces.Repositories;
using Fauno.Agenda.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Agenda.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AgendaDbContext _context;

        public AppointmentRepository(AgendaDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task<Appointment?> GetByIdAsync(Guid id) =>
            await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == id && a.RemovedAt == null);

        public async Task<IEnumerable<Appointment>> GetByVeterinarianIdAsync(Guid veterinarianId) =>
            await _context.Appointments
                .Where(a => a.VeterinarianId == veterinarianId && a.RemovedAt == null)
                .ToListAsync();

        public async Task<bool> HasConflictAsync(Guid veterinarianId, DateTime start, DateTime end) =>
            await _context.Appointments
                .AnyAsync(a =>
                    a.VeterinarianId == veterinarianId &&
                    a.RemovedAt == null &&
                    a.Status != AppointmentStatus.Cancelled &&
                    a.Start < end && a.End > start);
    
        public async Task<IEnumerable<Appointment>> GetByVeterinarianAndDateAsync(Guid veterinarianId, DateOnly date)
        {
            var start = date.ToDateTime(TimeOnly.MinValue);
            var end = date.ToDateTime(TimeOnly.MaxValue);

            return await _context.Appointments
                .Where(a =>
                    a.VeterinarianId == veterinarianId &&
                    a.RemovedAt == null &&
                    a.Status != AppointmentStatus.Cancelled &&
                    a.Start >= start && a.Start < end)
                .ToListAsync();
        }
        public async Task UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByOwnerIdAsync(Guid ownerId) =>
            await _context.Appointments
                .Where(a => a.OwnerId == ownerId && a.RemovedAt == null)
                .ToListAsync();
    }
}
