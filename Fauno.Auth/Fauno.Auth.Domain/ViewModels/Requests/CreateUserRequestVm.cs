using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Auth.Domain.ViewModels.Requests
{
    public class CreateUserRequestVm
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
