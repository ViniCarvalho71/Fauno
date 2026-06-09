using System;
using System.Threading.Tasks;
using Fauno.Register.Domain.Entities;
using Fauno.Register.Domain.Repositories;

namespace Fauno.Register.Application.UseCases;

public class ObterDonoPorIdUseCase
{
    private readonly IDonoRepository _repo;
    public ObterDonoPorIdUseCase(IDonoRepository repo) => _repo = repo;

    public async Task<Dono?> Run(Guid donoId)
    {
        return await _repo.ObterPorIdAsync(donoId);
    }
}
