using Fauno.Register.Application.DTOs.Requests;
using Fauno.Register.Application.Interfaces.Http;
using Fauno.Register.Domain.Entities;
using Fauno.Register.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace Fauno.Register.Application.UseCases;

public class CadastrarVeterinarioUseCase
{
    private readonly IVeterinarioRepository _vetRepository;
    private readonly IAuthGateway _authGateway;

    public CadastrarVeterinarioUseCase(IVeterinarioRepository vetRepository, IAuthGateway authGateway)
    {
        _vetRepository = vetRepository;
        _authGateway = authGateway;

    }

    public async Task<Veterinario> Run(CadastrarVeterinarioRequest request)
    {
        
        var cpfLimpo = request.Cpf.Replace(".", "").Replace("-", "");

        if (await _vetRepository.ExisteCpfAsync(cpfLimpo))
            throw new Exception("Já existe um veterinário cadastrado com este CPF.");
        var userId = await _authGateway.CreateUser(request.Email, request.Password);
        if (userId == Guid.Empty)
            throw new Exception("Falha ao criar usuário.");
        try
        {
            var vet = new Veterinario(userId, request.Nome, cpfLimpo, request.Crmv);
            await _vetRepository.SalvarAsync(vet);

            return vet;
        }
        catch (Exception ex)
        {
            _authGateway.DeleteUser(userId);
            throw new Exception ("Ocorreu um erro ao cadastrar o usuário: " + ex.Message);
        }
    }
}