using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fauno.Register.Domain.Entities;
using Fauno.Register.Domain.Repositories;

namespace Fauno.Register.Application.UseCases;

public class ListarPetsDoDonoUseCase
{
    private readonly IPetRepository _petRepository;
    public ListarPetsDoDonoUseCase(IPetRepository petRepository) => _petRepository = petRepository;

    public async Task<IEnumerable<Pet>> Run(Guid donoId)
    {
        return await _petRepository.BuscarPorDonoIdAsync(donoId);
    }
}