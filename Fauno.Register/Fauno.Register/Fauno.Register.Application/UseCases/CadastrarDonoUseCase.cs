using System;
using System.Threading.Tasks;
using Fauno.Register.Domain.Entities;
using Fauno.Register.Domain.Repositories;
using Fauno.Register.Application.DTOs.Requests;
using Fauno.Register.Application.Interfaces.Http;

namespace Fauno.Register.Application.UseCases;

public class CadastrarDonoUseCase
{
    private readonly IDonoRepository _donoRepository;
    private readonly IAuthGateway _authGateway;
    public CadastrarDonoUseCase(IDonoRepository donoRepository, IAuthGateway authGateway)
    {
        _donoRepository = donoRepository;
        _authGateway = authGateway;
    }

    public async Task<Dono> Run(CadastrarDonoRequest request)
    {
        var cpfLimpo = request.Cpf.Replace(".", "").Replace("-", "");

        if (await _donoRepository.ExisteCpfAsync(cpfLimpo))
            throw new Exception("Já existe um dono cadastrado com este CPF.");
        var userId = await _authGateway.CreateUser(request.Email, request.Password);
        if (userId == Guid.Empty)
            throw new Exception("Falha ao criar usuário.");

        try
        {
            var dono = new Dono(userId, request.Nome, cpfLimpo, request.Email);
            await _donoRepository.SalvarAsync(dono);

            return dono;
        }
        catch (Exception ex)
        {
            _authGateway.DeleteUser(userId);
            throw new Exception ("Ocorreu um erro ao cadastrar o usuário: " + ex.Message);
        }
    }
}