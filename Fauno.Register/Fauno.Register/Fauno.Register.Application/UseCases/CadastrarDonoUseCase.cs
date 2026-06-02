using System;
using System.Threading.Tasks;
using Fauno.Register.Domain.Entities;
using Fauno.Register.Domain.Repositories;

namespace Fauno.Register.Application.UseCases;

public class CadastrarDonoRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class CadastrarDonoUseCase
{
    private readonly IDonoRepository _donoRepository;

    public CadastrarDonoUseCase(IDonoRepository donoRepository)
    {
        _donoRepository = donoRepository;
    }

    public async Task<Dono> ExecutarAsync(CadastrarDonoRequest request)
    {
        var cpfLimpo = request.Cpf.Replace(".", "").Replace("-", "");

        if (await _donoRepository.ExisteCpfAsync(cpfLimpo))
            throw new Exception("Já existe um dono cadastrado com este CPF.");

        var dono = new Dono(request.Nome, cpfLimpo, request.Email);
        await _donoRepository.SalvarAsync(dono);

        return dono;
    }
}