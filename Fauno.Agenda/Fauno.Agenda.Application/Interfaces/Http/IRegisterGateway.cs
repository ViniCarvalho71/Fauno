using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Agenda.Application.Interfaces.Http
{
    public interface IRegisterGateway
    {
        Task<bool> OwnerExists(Guid ownerId);
        Task<bool> PetExists(Guid ownerId, Guid petId);
        Task<bool> VeterinarianExists(Guid veterinarianId);
        Task<Guid> GetVeterinarianIdByUserIdAsync(Guid userId);
        Task<Guid> GetOwnerIdByUserIdAsync(Guid userId);
        Task<string> GetVeterinarianNameById(Guid veterinarianId);
        Task<string> GetOwnerNameById(Guid ownerId);
        public Task<string> GetPetNameById(Guid petId);



    }
}
