using System.Net.Http.Json;
using Fauno.Agenda.Application.Interfaces.Http;
namespace Fauno.Agenda.Infrastructure.Http
{
    public class RegisterApiClient : IRegisterGateway
    {
        private readonly HttpClient _http;
        public RegisterApiClient(HttpClient http)
        {
            _http = http;
        }
        public async Task<bool> OwnerExists(Guid ownerId)
        {
            var response = await _http.GetFromJsonAsync<Dictionary<string, bool>>($"owners/{ownerId}/exists");

            return response?["existed"] ?? false;
        }
        public async Task<bool> PetExists(Guid ownerId, Guid petId)
        {
            var response = await _http.GetFromJsonAsync<Dictionary<string, bool>>($"owners/{ownerId}/{petId}/exists");

            return response?["existed"] ?? false;
        }

        public async Task<bool> VeterinarianExists(Guid veterinarianId)
        {
            //var response = await _http.GetFromJsonAsync<Dictionary<string, bool>>($"veterinarian/{veterinarianId}/exists");
            return true;
            //return response?["existed"] ?? false;
        }
    }
}
