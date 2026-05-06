using Fauno.Auth.Domain.ViewModels.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Auth.Domain.UseCases
{
    public class LoginUseCase
    {
        public LoginReponseVm Run()
        {
            return new LoginReponseVm();
        }
    }
}
