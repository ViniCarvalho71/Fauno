using System;
using System.Threading.Tasks;
using Fauno.Register.Domain.Repositories;

namespace Fauno.Register.Application.UseCases;

public class VerificarDonoExisteUseCase
{
    private readonly IDonoRepository _repo;
    public VerificarDonoExisteUseCase(IDonoRepository repo) => _repo = repo;

    public async Task<bool> Run(Guid donoId) => await _repo.ExistePorIdAsync(donoId);
}