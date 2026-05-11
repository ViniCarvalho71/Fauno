using Fauno.Auth.Application.Interfaces;
using Fauno.Auth.Domain.ValueObjects;
using Fauno.Auth.Domain.ViewModels.Requests;
using Fauno.Auth.Domain.ViewModels.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Fauno.Auth.Application.UseCases
{
    public class LoginUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher _passwordHasher;

        public LoginUseCase(IUserRepository userRepository, IConfiguration configuration, IPasswordHasher passwordHashe )
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _passwordHasher = passwordHashe;
        }

        public LoginReponseVm Run(LoginRequestVm loginData)
        {
            var password = new Password(loginData.Password);

            var user = _userRepository.GetUserByEmail(loginData.Email);

            if (user == null)
                return null;

            var passwordValid = _passwordHasher.Verify(
                password.Value,
                user.Password.Value
            );

            if (!passwordValid)
                return null;

            var secretKey = _configuration["JwtSettings:Secret"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var expirationInMinutes = int.Parse(_configuration["JwtSettings:ExpirationInMinutes"] ?? "60");

            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, loginData.Email),
                    new Claim(ClaimTypes.Email, loginData.Email),
                    new Claim("google_access_token", user.GoogleToken)
                }),
                Expires = DateTime.UtcNow.AddMinutes(expirationInMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginReponseVm
            {
                Token = tokenHandler.WriteToken(token),
                ExpiresAt = tokenDescriptor.Expires.Value
            };
        }
    }
}
