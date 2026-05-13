using Fauno.Auth.Application.Interfaces;
using Fauno.Auth.Domain.Entities;
using Fauno.Auth.Domain.ValueObjects;
using Fauno.Auth.Infrastructure.Data.Context;
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

        public long CreateUser(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Add(user);

            _context.SaveChanges();

            return user.Id;
        }

        public User GetUserByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email.Value == email);

            return user;
        }

        public User GetUsertById(long id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            return user;
        }

    }
}
