using System;
using System.Threading.Tasks;
using Fauno.Register.Domain.Repositories;

namespace Fauno.Register.Application.UseCases;

public class VerificarPetExisteUseCase
{
    private readonly IPetRepository _repo;
    public VerificarPetExisteUseCase(IPetRepository repo) => _repo = repo;

    public async Task<bool> Run(Guid petId, Guid ownerId) => await _repo.ExistePorIdAsync(petId, ownerId);
}