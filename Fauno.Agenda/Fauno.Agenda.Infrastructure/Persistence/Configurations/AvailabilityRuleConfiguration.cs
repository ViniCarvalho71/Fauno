using Fauno.Agenda.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fauno.Agenda.Infrastructure.Persistence.Configurations
{
    public class AvailabilityRuleConfiguration : IEntityTypeConfiguration<AvailabilityRule>
    {
        public void Configure(EntityTypeBuilder<AvailabilityRule> builder)
        {
            builder.ToTable("AvailabilityRules");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.VeterinarianId).IsRequired();
            builder.Property(r => r.SlotDurationMinutes).IsRequired();

            // PauseWindow — simples, vai como colunas na mesma tabela
            builder.OwnsOne(r => r.Pause, pause =>
            {
                pause.Property(p => p.Start).HasColumnName("PauseStart");
                pause.Property(p => p.End).HasColumnName("PauseEnd");
            });

            // Recurrence — complexo com coleções, vai como JSON
            builder.OwnsOne(r => r.Recurrence, recurrence =>
            {
                recurrence.ToJson();
            });
        }
    }
}