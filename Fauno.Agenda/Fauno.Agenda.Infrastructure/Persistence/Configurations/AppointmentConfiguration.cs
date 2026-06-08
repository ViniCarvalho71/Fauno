using Fauno.Agenda.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fauno.Agenda.Infrastructure.Persistence.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointments");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Title).HasMaxLength(200);
            builder.Property(a => a.Description).HasMaxLength(1000);
            builder.Property(a => a.Status).HasConversion<string>().HasMaxLength(50);
            builder.Property(a => a.AppointmentType).HasConversion<string>().HasMaxLength(50);
            builder.Property(a => a.VeterinarianId).IsRequired();
            builder.Property(a => a.OwnerId).IsRequired();
            builder.Property(a => a.PetId).IsRequired();
            builder.Property(a => a.Start).IsRequired();
            builder.Property(a => a.End).IsRequired();
        }
    }
}