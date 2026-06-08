using System.Threading.Tasks;
using Fauno.Register.Domain.Entities;

namespace Fauno.Register.Domain.Repositories;

public interface IVeterinarioRepository
{
    Task SalvarAsync(Veterinario vet);
    Task<bool> ExisteCpfAsync(string cpf);
    Task<Guid?> ObterIdPorUserIdAsync(Guid userId);
    Task<bool> ExistePorIdAsync(Guid userId);
}