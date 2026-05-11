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

        public User GetUserByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == new Email(email));

            return user;
        }

        public User GetUsertById(long id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            return user;
        }

        public void UpdateUser(User user)
        {
            try
            {
                var existingUser = GetUsertById(user.Id);
                if (existingUser == null)
                {
                    throw new Exception("Usuário não encontrado");
                }

                existingUser.Email = user.Email;
                existingUser.GoogleToken = user.GoogleToken;
                existingUser.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar o usuário: " + ex.Message);
            }
        }
    }
}
