using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Fauno.Register.Application.Interfaces.Http;
namespace Fauno.Agenda.Infrastructure.Http

{
    public class RegisterApiClient : IAuthGateway
    {
        private readonly HttpClient _http;
        public RegisterApiClient(
            HttpClient http
            )
        {
            _http = http;
        }
        public async Task<Guid> CreateUser(string email, string password)
        {
            var payload = new
            {
                Email = email,
                Password = password
            };
            var response = await _http.PostAsJsonAsync("/api/v1/auth/create-user", payload);
            
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<CreateUserResponse>();
            return result.Id;
        }

        

    }
}
