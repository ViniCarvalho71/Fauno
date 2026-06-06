using System;
using Fauno.Register.Domain.ValueObjects;

namespace Fauno.Register.Domain.Entities;

public class Dono : EntityBase
{
    public Guid UserId { get; private set; }
    public string Nome { get; private set; }
    public Cpf Cpf { get; private set; }
    public string Email { get; private set; }

    public Dono(Guid userId, string nome, string cpf, string email) : base()
    {
        UserId = userId;
        Nome = nome;
        Cpf = new Cpf(cpf);
        Email = email;
    }
    
    protected Dono() : base() { }
}