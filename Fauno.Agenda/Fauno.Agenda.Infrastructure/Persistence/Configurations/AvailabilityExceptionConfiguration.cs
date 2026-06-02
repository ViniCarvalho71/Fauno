using Fauno.Agenda.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fauno.Agenda.Infrastructure.Persistence.Configurations
{
    public class AvailabilityExceptionConfiguration : IEntityTypeConfiguration<AvailabilityException>
    {
        public void Configure(EntityTypeBuilder<AvailabilityException> builder)
        {
            builder.ToTable("AvailabilityExceptions");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.VeterinarianId).IsRequired();
            builder.Property(e => e.Date).IsRequired();
            builder.Property(e => e.Reason).HasMaxLength(500);
        }
    }
}