using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Fauno.Register.Domain.Entities;
using Fauno.Register.Domain.Repositories;
using Fauno.Register.Infrastructure.Data.Context;

namespace Fauno.Register.Infrastructure.Repositories;

public class DonoRepository : IDonoRepository
{
    private readonly Context _context;

    public DonoRepository(Context context)
    {
        _context = context;
    }

    public async Task SalvarAsync(Dono dono)
    {
        await _context.Donos.AddAsync(dono);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExisteCpfAsync(string cpf)
    {
        return await _context.Donos.AnyAsync(d => d.Cpf.Numero == cpf);
    }
    
    public async Task<Guid?> ObterIdPorUserIdAsync(Guid userId)
    {
        var dono = await _context.Donos.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == userId);
        return dono?.Id;
    }

    public async Task<bool> ExistePorIdAsync(Guid id)
    {
        return await _context.Donos.AnyAsync(d => d.Id== id);
    }
}