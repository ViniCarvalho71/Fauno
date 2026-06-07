using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Register.Application.Interfaces.Http
{
    public interface IAuthGateway
    {
        Task<Guid> CreateUser(string email, string password);
    }
}
