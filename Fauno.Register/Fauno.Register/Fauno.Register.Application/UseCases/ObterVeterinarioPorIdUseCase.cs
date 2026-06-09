using System;
using System.Threading.Tasks;
using Fauno.Register.Domain.Entities;
using Fauno.Register.Domain.Repositories;

namespace Fauno.Register.Application.UseCases;

public class ObterVeterinarioPorIdUseCase
{
    private readonly IVeterinarioRepository _repo;
    public ObterVeterinarioPorIdUseCase(IVeterinarioRepository repo) => _repo = repo;

    public async Task<Veterinario?> Run(Guid veterinarioId)
    {
        return await _repo.ObterPorIdAsync(veterinarioId);
    }
}
