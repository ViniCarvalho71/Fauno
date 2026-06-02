using Fauno.Agenda.Domain.Entities;
using Fauno.Agenda.Domain.Interfaces.Repositories;
using Fauno.Agenda.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Agenda.Infrastructure.Repositories
{
    public class AvailabilityRuleRepository : IAvailabilityRuleRepository
    {
        private readonly AgendaDbContext _context;

        public AvailabilityRuleRepository(AgendaDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AvailabilityRule rule)
        {
            await _context.AvailabilityRules.AddAsync(rule);
            await _context.SaveChangesAsync();
        }

        public async Task<AvailabilityRule?> GetByIdAsync(Guid id) =>
            await _context.AvailabilityRules
                .FirstOrDefaultAsync(r => r.Id == id && r.RemovedAt == null);

        public async Task<IEnumerable<AvailabilityRule>> GetByVeterinarianIdAsync(Guid veterinarianId) =>
            await _context.AvailabilityRules
                .Where(r => r.VeterinarianId == veterinarianId && r.RemovedAt == null)
                .ToListAsync();
        public async Task<IEnumerable<AvailabilityRule>> GetActiveForDateAsync(Guid veterinarianId)=>
            await _context.AvailabilityRules
                .Where(r => r.VeterinarianId == veterinarianId && r.RemovedAt == null)
                .ToListAsync();
        public async Task UpdateAsync(AvailabilityRule rule)
        {
            _context.AvailabilityRules.Update(rule);
            await _context.SaveChangesAsync();
        }

    }
}
