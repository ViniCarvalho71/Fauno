using System;

namespace Fauno.Register.Domain.Entities;

public class Pet
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Especie { get; private set; }
    public string Raca { get; private set; }
    public Guid DonoId { get; private set; }

    public Pet(string nome, string especie, string raca, Guid donoId)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Especie = especie;
        Raca = raca;
        DonoId = donoId;
    }

    public void AtualizarDados(string nome, string especie, string raca)
    {
        Nome = nome;
        Especie = especie;
        Raca = raca;
    }

    protected Pet() { }
}