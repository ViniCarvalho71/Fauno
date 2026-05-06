using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Auth.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        bool Login(string username, string password);
    }
}
