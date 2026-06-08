using System;
using System.Threading.Tasks;
using Fauno.Register.Domain.Entities;
using Fauno.Register.Domain.Repositories;
using Fauno.Register.Application.DTOs;

namespace Fauno.Register.Application.UseCases;

public class AtualizarPetUseCase
{
    private readonly IPetRepository _petRepository;
    public AtualizarPetUseCase(IPetRepository petRepository) => _petRepository = petRepository;

    public async Task<Pet> Run(Guid petId, AtualizarPetRequest request)
    {
        var pet = await _petRepository.BuscarPorIdAsync(petId);
        if (pet == null) throw new Exception("Pet não encontrado.");

        pet.AtualizarDados(request.Nome, request.Especie, request.Raca);
        await _petRepository.AtualizarAsync(pet);
        return pet;
    }
}