using System;
using System.Threading.Tasks;
using Fauno.Register.Domain.Entities;
using Fauno.Register.Domain.Repositories;
using Fauno.Register.Application.DTOs;

namespace Fauno.Register.Application.UseCases;

public class CadastrarPetUseCase
{
    private readonly IPetRepository _petRepository;
    private readonly IDonoRepository _donoRepository;

    public CadastrarPetUseCase(IPetRepository petRepository, IDonoRepository donoRepository)
    {
        _petRepository = petRepository;
        _donoRepository = donoRepository;
    }

    public async Task<Pet> Run(CadastrarPetRequest request)
    {
        var pet = new Pet(request.Nome, request.Especie, request.Raca, request.DonoId);
        await _petRepository.SalvarAsync(pet);
        return pet;
    }
}