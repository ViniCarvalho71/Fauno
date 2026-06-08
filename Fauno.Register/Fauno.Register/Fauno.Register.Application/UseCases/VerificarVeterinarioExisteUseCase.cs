using System;
using System.Threading.Tasks;
using Fauno.Register.Domain.Repositories;

namespace Fauno.Register.Application.UseCases;

public class VerificarVeterinarioExisteUseCase
{
    private readonly IVeterinarioRepository _repo;
    public VerificarVeterinarioExisteUseCase(IVeterinarioRepository repo) => _repo = repo;

    public async Task<bool> Run(Guid veterinarioId) => await _repo.ExistePorIdAsync(veterinarioId);
}