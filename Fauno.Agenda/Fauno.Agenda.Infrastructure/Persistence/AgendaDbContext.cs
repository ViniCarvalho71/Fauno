using Fauno.Agenda.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fauno.Agenda.Infrastructure.Persistence
{
    public class AgendaDbContext : DbContext
    {
        public AgendaDbContext(DbContextOptions<AgendaDbContext> options) : base(options) { }

        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<AvailabilityRule> AvailabilityRules => Set<AvailabilityRule>();
        public DbSet<AvailabilityException> AvailabilityExceptions => Set<AvailabilityException>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AgendaDbContext).Assembly);
        }
    }
}