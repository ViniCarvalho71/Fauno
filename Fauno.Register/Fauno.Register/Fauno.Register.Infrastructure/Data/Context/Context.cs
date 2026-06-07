using Fauno.Register.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fauno.Register.Infrastructure.Data.Context
{
    public class Context : DbContext
    {
        public DbSet<Dono> Donos => Set<Dono>();
        public DbSet<Pet> Pets => Set<Pet>();
        public DbSet<Veterinario> Veterinarios => Set<Veterinario>();

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dono>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.UserId).IsRequired();
                entity.Property(x => x.CreatedAt).IsRequired();
                entity.Property(x => x.RemovedAt);
                
                entity.OwnsOne(x => x.Cpf, cpf =>
                {
                    cpf.Property(c => c.Numero)
                        .HasColumnName("Cpf")
                        .IsRequired();
                });

                entity.Property(x => x.Nome).IsRequired();
                entity.Property(x => x.Email).IsRequired();
            });
            
            modelBuilder.Entity<Pet>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne<Dono>()
                    .WithMany()
                    .HasForeignKey(p => p.DonoId);
            });

            modelBuilder.Entity<Veterinario>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.UserId).IsRequired();
                entity.Property(x => x.CreatedAt).IsRequired();
                entity.Property(x => x.RemovedAt);

                entity.OwnsOne(x => x.Cpf, cpf =>
                {
                    cpf.Property(c => c.Numero)
                        .HasColumnName("Cpf")
                        .IsRequired();
                });

                entity.Property(x => x.Nome).IsRequired();
                entity.Property(x => x.Crmv).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}