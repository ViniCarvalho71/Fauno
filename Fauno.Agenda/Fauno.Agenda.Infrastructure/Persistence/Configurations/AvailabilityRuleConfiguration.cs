using System.Text.Json;
using Fauno.Agenda.Domain.Entities;
using Fauno.Agenda.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fauno.Agenda.Infrastructure.Persistence.Configurations
{
    public class AvailabilityRuleConfiguration : IEntityTypeConfiguration<AvailabilityRule>
    {
        public void Configure(EntityTypeBuilder<AvailabilityRule> builder)
        {
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.General);

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

            builder.Property(r => r.Recurrence)
                .HasColumnType("json")
                .HasConversion(
                    recurrence => JsonSerializer.Serialize(recurrence, jsonOptions),
                    json => JsonSerializer.Deserialize<Recurrence>(json, jsonOptions)!)
                .Metadata.SetValueComparer(new ValueComparer<Recurrence>(
                    (left, right) => JsonSerializer.Serialize(left, jsonOptions) == JsonSerializer.Serialize(right, jsonOptions),
                    value => JsonSerializer.Serialize(value, jsonOptions).GetHashCode(),
                    value => JsonSerializer.Deserialize<Recurrence>(JsonSerializer.Serialize(value, jsonOptions), jsonOptions)!));
        }
    }
}