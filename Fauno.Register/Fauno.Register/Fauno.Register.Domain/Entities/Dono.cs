using System;
using Fauno.Register.Domain.ValueObjects;

namespace Fauno.Register.Domain.Entities;

public class Dono
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public Cpf Cpf { get; private set; }
    public string Email { get; private set; }

    public Dono(string nome, string cpf, string email)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Cpf = new Cpf(cpf);
        Email = email;
    }
    
    protected Dono() { }
}