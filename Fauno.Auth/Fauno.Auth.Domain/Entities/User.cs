using Fauno.Auth.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Auth.Domain.Entities
{
    public class User : EntityBase
    {
        public Email Email { get; private set; }

        public string PasswordHash { get; private set; }

        protected User()
        {
        }

        public User(
            Email email,
            string passwordHash)
        {
            Email = email;
            PasswordHash = passwordHash;
        }
    }
}
