namespace Fauno.Register.Application.DTOs.Requests;

public class CadastrarDonoRequest
{
    public Guid UserId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}