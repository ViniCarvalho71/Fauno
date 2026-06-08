using Fauno.Auth.Application.Interfaces;
using Fauno.Auth.Domain.Entities;
using Fauno.Auth.Domain.ValueObjects;
using Fauno.Auth.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Auth.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;

        public UserRepository(Context context)
        {
            _context = context;
        }

        public Guid CreateUser(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Add(user);

            _context.SaveChanges();

            return user.Id;
        }

        public void DeleteUser(Guid id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public User GetUserByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email.Value == email);
            return user;
        }

        public User GetUsertById(Guid id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            return user;
        }

    }
}
