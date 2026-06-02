using System;

namespace Fauno.Register.Domain.ValueObjects;

public class Cpf
{
    public string Numero { get; private set; }

    public Cpf(string numero)
    {
        var limpo = numero?.Replace(".", "").Replace("-", "");
        
        if (string.IsNullOrWhiteSpace(limpo) || limpo.Length != 11)
            throw new ArgumentException("CPF inválido. Deve conter 11 dígitos.");
            
        Numero = limpo; 
    }
}