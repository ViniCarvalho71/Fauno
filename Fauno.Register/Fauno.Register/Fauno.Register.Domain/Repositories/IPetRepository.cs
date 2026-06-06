using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fauno.Register.Domain.Entities;

namespace Fauno.Register.Domain.Repositories;

public interface IPetRepository
{
    Task SalvarAsync(Pet pet);
    Task AtualizarAsync(Pet pet);
    Task<Pet?> BuscarPorIdAsync(Guid id);
    Task<IEnumerable<Pet>> BuscarPorDonoIdAsync(Guid donoId);
    Task <bool> ExistePorIdAsync(Guid id);
}