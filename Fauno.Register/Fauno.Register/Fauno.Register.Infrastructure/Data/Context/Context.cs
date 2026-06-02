using Fauno.Register.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Register.Infrastructure.Data.Context
{
    public class Context : DbContext
    {
        public DbSet<Dono> Donos { get; set; }
        public DbSet<Veterinario> Veterinarios { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dono>(entity =>
            {
                entity.HasKey(x => x.Id);

                // Molde exato do seu Fauno.Auth
                entity.OwnsOne(x => x.Cpf, cpf =>
                {
                    cpf.Property(c => c.Numero)
                        .HasColumnName("Cpf")
                        .IsRequired();
                });

                entity.Property(x => x.Nome).IsRequired();
                entity.Property(x => x.Email).IsRequired();
            });

            modelBuilder.Entity<Veterinario>(entity =>
            {
                entity.HasKey(x => x.Id);

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