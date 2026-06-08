using System;
using System.Threading.Tasks;
using Fauno.Register.Domain.Repositories;

namespace Fauno.Register.Application.UseCases;

public class ObterVeterinarioIdPorUserIdUseCase
{
    private readonly IVeterinarioRepository _repo;
    public ObterVeterinarioIdPorUserIdUseCase(IVeterinarioRepository repo) => _repo = repo;

    public async Task<Guid?> Run(Guid userId) => await _repo.ObterIdPorUserIdAsync(userId);
}