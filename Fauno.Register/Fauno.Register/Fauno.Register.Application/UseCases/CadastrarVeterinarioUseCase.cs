using System;
using System.Threading.Tasks;
using Fauno.Register.Domain.Entities;
using Fauno.Register.Domain.Repositories;

namespace Fauno.Register.Application.UseCases;

public class CadastrarVeterinarioRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Crmv { get; set; } = string.Empty;
}

public class CadastrarVeterinarioUseCase
{
    private readonly IVeterinarioRepository _vetRepository;

    public CadastrarVeterinarioUseCase(IVeterinarioRepository vetRepository)
    {
        _vetRepository = vetRepository;
    }

    public async Task<Veterinario> Run(CadastrarVeterinarioRequest request)
    {
        var cpfLimpo = request.Cpf.Replace(".", "").Replace("-", "");

        if (await _vetRepository.ExisteCpfAsync(cpfLimpo))
            throw new Exception("Já existe um veterinário cadastrado com este CPF.");

        var vet = new Veterinario(request.Nome, cpfLimpo, request.Crmv);
        await _vetRepository.SalvarAsync(vet);

        return vet;
    }
}