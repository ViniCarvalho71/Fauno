using Fauno.Auth.Application.Interfaces;
using Fauno.Auth.Domain.Entities;
using Fauno.Auth.Domain.ValueObjects;
using Fauno.Auth.Domain.ViewModels.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Auth.Application.UseCases
{
    public class CreateUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserUseCase(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public long Run(CreateUserRequestVm request)
        {
            try
            {
                var existingUser =
               _userRepository.GetUserByEmail(request.Email);

                if (existingUser != null)
                    throw new Exception("Usuário já existe");

                var email = new Email(request.Email);
                var password = new Password(request.Password);

                var passwordHash =
                    _passwordHasher.Hash(password.Value);

                var user = new User(
                    email,
                    passwordHash
                );

                var userId =
                    _userRepository.CreateUser(user);

                return userId;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }
    }
}
