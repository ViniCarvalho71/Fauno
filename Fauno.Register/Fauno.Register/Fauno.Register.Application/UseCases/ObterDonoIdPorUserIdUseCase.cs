using System;
using System.Threading.Tasks;
using Fauno.Register.Domain.Repositories;

namespace Fauno.Register.Application.UseCases;

public class ObterDonoIdPorUserIdUseCase
{
    private readonly IDonoRepository _repo;
    public ObterDonoIdPorUserIdUseCase(IDonoRepository repo) => _repo = repo;

    public async Task<Guid?> Run(Guid userId) => await _repo.ObterIdPorUserIdAsync(userId);
    
}