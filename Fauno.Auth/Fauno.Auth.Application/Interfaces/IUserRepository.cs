using Fauno.Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Auth.Application.Interfaces
{
    public interface IUserRepository
    {
        User GetUserByEmail(string email);
        User GetUsertById(long id);
        void UpdateUser(User user);
    }
}
