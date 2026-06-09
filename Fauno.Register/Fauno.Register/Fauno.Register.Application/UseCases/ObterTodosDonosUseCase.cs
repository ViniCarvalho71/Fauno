using System;
using System.Threading.Tasks;
using Fauno.Register.Domain.Entities;
using Fauno.Register.Domain.Repositories;

namespace Fauno.Register.Application.UseCases;

public class ObterTodosDonosUseCase
{
    private readonly IDonoRepository _repo;
    public ObterTodosDonosUseCase(IDonoRepository repo) => _repo = repo;

    public async Task<IEnumerable<Dono>> Run() => await _repo.ObterTodosAsync();
}