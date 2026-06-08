using System;

namespace Fauno.Register.Application.DTOs;

public class CadastrarPetRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Especie { get; set; } = string.Empty;
    public string Raca { get; set; } = string.Empty;
    public Guid DonoId { get; set; }
}