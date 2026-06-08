namespace Fauno.Register.Application.DTOs.Requests;

public class CadastrarVeterinarioRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Crmv { get; set; } = string.Empty;
    public string Email { get; set; }
    public string Password { get; set; }
}