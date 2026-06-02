using System;
using Fauno.Register.Domain.ValueObjects;

namespace Fauno.Register.Domain.Entities;

public class Veterinario
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public Cpf Cpf { get; private set; }
    public string Crmv { get; private set; }

    public Veterinario(string nome, string cpf, string crmv)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Cpf = new Cpf(cpf);
        Crmv = crmv;
    }
    
    protected Veterinario() { }
}