using System.Threading.Tasks;
using Fauno.Register.Domain.Entities;

namespace Fauno.Register.Domain.Repositories;

public interface IDonoRepository
{
    Task SalvarAsync(Dono dono);
    Task<bool> ExisteCpfAsync(string cpf);
    Task<Guid?> ObterIdPorUserIdAsync(Guid userId);
    Task<bool> ExistePorIdAsync(Guid id);
    Task<Dono?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Dono>> ObterTodosAsync();
}