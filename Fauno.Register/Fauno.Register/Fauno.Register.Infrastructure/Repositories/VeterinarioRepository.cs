using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Fauno.Register.Domain.Entities;
using Fauno.Register.Domain.Repositories;
using Fauno.Register.Infrastructure.Data.Context;

namespace Fauno.Register.Infrastructure.Repositories;

public class VeterinarioRepository : IVeterinarioRepository
{
    private readonly Context _context;

    public VeterinarioRepository(Context context)
    {
        _context = context;
    }

    public async Task SalvarAsync(Veterinario vet)
    {
        await _context.Veterinarios.AddAsync(vet);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExisteCpfAsync(string cpf)
    {
        return await _context.Veterinarios.AnyAsync(v => v.Cpf.Numero == cpf);
    }
    
    public async Task<Guid?> ObterIdPorUserIdAsync(Guid userId)
    {
        var dono = await _context.Veterinarios.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == userId);
        return dono?.Id;
    }

    public async Task<bool> ExistePorIdAsync(Guid userId)
    {
        return await _context.Veterinarios.AnyAsync(d => d.UserId == userId);
    }
}