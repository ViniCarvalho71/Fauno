using System.Net.Http.Json;
using Fauno.Agenda.Application.Interfaces.Http;
namespace Fauno.Agenda.Infrastructure.Http

{
    public class RegisterApiClient : IRegisterGateway
    {
        private readonly HttpClient _http;  
        public RegisterApiClient(
            HttpClient http
            )
        {
            _http = http;
        }

        public async Task<Guid> GetOwnerIdByUserIdAsync(Guid userId)
        {

            var response = await _http.GetFromJsonAsync<Dictionary<string, Guid>>(
                $"donos/usuario/{userId}/id");

            return response?["ownerId"] ?? Guid.Empty;
        }

        public async Task<Guid> GetVeterinarianIdByUserIdAsync(Guid userId)
        {
            var response = await _http.GetFromJsonAsync<Dictionary<string, Guid>>(
                 $"veterinarios/usuario/{userId}/id");


            return response?["veterinarianId"] ?? Guid.Empty;
        }

        public async Task<bool> OwnerExists(Guid ownerId) =>
            await _http.GetFromJsonAsync<bool>($"donos/usuario/{ownerId}/existe");

        public async Task<bool> PetExists(Guid ownerId, Guid petId)=>
            await _http.GetFromJsonAsync<bool>($"pets/dono/{ownerId}/{petId}/existe");
        

        public async Task<bool> VeterinarianExists(Guid veterinarianId)=>
            await _http.GetFromJsonAsync<bool>($"veterinarios/usuario/{veterinarianId}/existe");
        
    }
}
