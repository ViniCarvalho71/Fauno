using System;
using System.Threading.Tasks;
using Fauno.Register.Domain.Repositories;

namespace Fauno.Register.Application.UseCases;

public class ObterVeterinariosUseCase
{
    private readonly IVeterinarioRepository _repo;
    public ObterVeterinariosUseCase(IVeterinarioRepository repo) => _repo = repo;

    public async Task<IEnumerable<Veterinario>> Run() => await _repo.ObterTodosAsync();
}