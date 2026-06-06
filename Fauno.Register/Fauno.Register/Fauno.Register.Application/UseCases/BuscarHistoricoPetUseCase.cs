using System;
using System.Threading.Tasks;
using Fauno.Register.Domain.Repositories;

namespace Fauno.Register.Application.UseCases;

public class BuscarHistoricoPetUseCase
{
    private readonly IPetRepository _petRepository;
    public BuscarHistoricoPetUseCase(IPetRepository petRepository) => _petRepository = petRepository;

    public async Task<object> Run(Guid petId)
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