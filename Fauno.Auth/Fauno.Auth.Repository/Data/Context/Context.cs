using Fauno.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Auth.Infrastructure.Data.Context
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.OwnsOne(x => x.Email, email =>
                {
                    email.Property(e => e.Value)
                        .HasColumnName("Email")
                        .IsRequired();
                });

                entity.Property(x => x.PasswordHash)
                    .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
