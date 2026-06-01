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
    }
}
