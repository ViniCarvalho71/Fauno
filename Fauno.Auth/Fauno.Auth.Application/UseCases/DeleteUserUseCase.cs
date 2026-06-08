using Fauno.Auth.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Auth.Application.UseCases
{
    public class DeleteUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Run(Guid id)
        {
            try
            {
                var user = _userRepository.GetUsertById(id);
                if (user == null)
                    throw new Exception("Usuário não encontrado");
                _userRepository.DeleteUser(id);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
