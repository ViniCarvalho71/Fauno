using Fauno.Auth.Domain.Entities;
using Fauno.Auth.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Auth.Application.UseCases
{
    public class SaveGoogleTokenUseCase
    {
        private readonly IUserRepository _userRepository;
        public SaveGoogleTokenUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Run(long id)
        {
            try
            {
                User user = _userRepository.GetUsertById(id);

                _userRepository.UpdateUser(user);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            
        }
    }
}
