using Fauno.Auth.Application.UseCases;
using Fauno.Auth.Domain.ViewModels.Requests;
using Fauno.Auth.Domain.ViewModels.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fauno.Auth.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly LoginUseCase _loginUseCase;
        private readonly CreateUserUseCase _createUserUseCase;
        private readonly DeleteUserUseCase _deleteUserUseCase;
        public UserController(LoginUseCase loginUseCase, CreateUserUseCase createUserUseCase, DeleteUserUseCase deleteUserUseCase)
        {
            _loginUseCase = loginUseCase;
            _createUserUseCase = createUserUseCase;
            _deleteUserUseCase = deleteUserUseCase;
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginRequestVm loginData)
        {
            try
            {
                LoginReponseVm response = _loginUseCase.Run(loginData);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUser(CreateUserRequestVm request)
        {
            try
            {
                var userId = _createUserUseCase.Run(request);

                return Ok(new
                {
                    Id = userId
                });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("DeleteUser/{id}")]
        public IActionResult DeleteUser(Guid id)
        {
            try
            {
                _deleteUserUseCase.Run(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
