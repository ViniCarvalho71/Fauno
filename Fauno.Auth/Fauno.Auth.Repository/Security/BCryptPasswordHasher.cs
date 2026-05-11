using BCrypt.Net;
using Fauno.Auth.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Auth.Infrastructure.Security
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
