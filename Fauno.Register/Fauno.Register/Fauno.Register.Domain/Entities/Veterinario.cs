using System;
using Fauno.Register.Domain.ValueObjects;

namespace Fauno.Register.Domain.Entities;

public class Veterinario : EntityBase
{
    public Guid UserId { get; private set; }
    public string Nome { get; private set; }
    public Cpf Cpf { get; private set; }
    public string Crmv { get; private set; }

    public Veterinario(Guid userId, string nome, string cpf, string crmv) : base()
    {
        UserId = userId;
        Nome = nome;
        Cpf = new Cpf(cpf);
        Crmv = crmv;
    }
    
    protected Veterinario() : base() { }
}