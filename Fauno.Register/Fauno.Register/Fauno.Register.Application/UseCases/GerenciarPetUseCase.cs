using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fauno.Register.Domain.Entities;
using Fauno.Register.Domain.Repositories;
using Fauno.Register.Application.DTOs;

namespace Fauno.Register.Application.UseCases;

public class GerenciarPetUseCase
{
    private readonly IPetRepository _petRepository;

    public GerenciarPetUseCase(IPetRepository petRepository)
    {
        _petRepository = petRepository;
    }

    public async Task<IEnumerable<Pet>> ListarPorDonoAsync(Guid donoId)
    {
        return await _petRepository.BuscarPorDonoIdAsync(donoId);
    }

    public async Task<Pet> AtualizarAsync(Guid petId, AtualizarPetRequest request)
    {
        var pet = await _petRepository.BuscarPorIdAsync(petId);
        if (pet == null) throw new Exception("Pet não encontrado.");

        pet.AtualizarDados(request.Nome, request.Especie, request.Raca);
        await _petRepository.AtualizarAsync(pet);
        return pet;
    }

    public async Task<object> BuscarHistoricoAsync(Guid petId)
    {
        var pet = await _petRepository.BuscarPorIdAsync(petId);
        if (pet == null) throw new Exception("Pet não encontrado.");

        return new 
        {
            Pet = pet.Nome,
            Historico = new[] 
            {
                new { Data = DateTime.Now.AddDays(-30), Evento = "Cadastro inicial no sistema" },
                new { Data = DateTime.Now.AddDays(-10), Evento = "Atualização de peso e vacina" }
            }
        };
    }
}