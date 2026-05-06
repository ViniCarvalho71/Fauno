using Fauno.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Auth.Infrastructure.Data.Context
{
    public class Context : DbContext
    {
        public DbSet<User> Users;

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(usr => usr.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
