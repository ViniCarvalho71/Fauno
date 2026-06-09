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

        public async Task<string> GetOwnerNameById(Guid ownerId)
        {
            var owner = await _http.GetFromJsonAsync<OwnerResponse>(
                $"donos/{ownerId}");

            return owner?.Nome ?? string.Empty;
        }

        public async Task<Guid> GetVeterinarianIdByUserIdAsync(Guid userId)
        {
            var response = await _http.GetFromJsonAsync<Dictionary<string, Guid>>(
                 $"veterinarios/usuario/{userId}/id");


            return response?["veterinarianId"] ?? Guid.Empty;
        }

        public async Task<string> GetVeterinarianNameById(Guid veterinarianId)
        {
            var veterinarian = await _http.GetFromJsonAsync<VeterinarianResponse>(
                $"veterinarios/{veterinarianId}");

            return veterinarian?.Nome ?? string.Empty;
        }
        public async Task<string> GetPetNameById(Guid petId)
        {
            var pet = await _http.GetFromJsonAsync<PetResponse>(
                $"pets/{petId}/historico");
            return pet?.Pet ?? string.Empty;
        }

        public async Task<bool> OwnerExists(Guid ownerId) =>
            await _http.GetFromJsonAsync<bool>($"donos/usuario/{ownerId}/existe");

        public async Task<bool> PetExists(Guid ownerId, Guid petId)=>
            await _http.GetFromJsonAsync<bool>($"pets/dono/{ownerId}/{petId}/existe");
        

        public async Task<bool> VeterinarianExists(Guid veterinarianId)=>
            await _http.GetFromJsonAsync<bool>($"veterinarios/usuario/{veterinarianId}/existe");
        
    }
}
