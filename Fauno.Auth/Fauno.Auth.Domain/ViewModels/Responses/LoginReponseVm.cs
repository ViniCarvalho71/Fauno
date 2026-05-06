using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Auth.Domain.ViewModels.Responses
{
    public class LoginReponseVm
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
