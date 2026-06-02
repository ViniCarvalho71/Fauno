using Fauno.Agenda.Domain.Entities;
using Fauno.Agenda.Domain.Interfaces.Repositories;
using Fauno.Agenda.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Agenda.Infrastructure.Repositories
{
    public class AvailabilityExceptionRepository : IAvailabilityExceptionRepository
    {
        private readonly AgendaDbContext _context;

        public AvailabilityExceptionRepository(AgendaDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AvailabilityException exception)
        {
            await _context.AvailabilityExceptions.AddAsync(exception);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AvailabilityException>> GetByVeterinarianIdAsync(Guid veterinarianId) =>
            await _context.AvailabilityExceptions
                .Where(e => e.VeterinarianId == veterinarianId)
                .ToListAsync();
        // AvailabilityExceptionRepository
        public async Task<bool> ExistsForDateAsync(Guid veterinarianId, DateOnly date) =>
            await _context.AvailabilityExceptions
                .AnyAsync(e => e.VeterinarianId == veterinarianId && e.Date == date);
    }
}
