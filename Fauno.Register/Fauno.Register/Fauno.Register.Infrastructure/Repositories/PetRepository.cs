using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Fauno.Register.Domain.Entities;
using Fauno.Register.Domain.Repositories;
using Fauno.Register.Infrastructure.Data.Context;

namespace Fauno.Register.Infrastructure.Repositories;

public class PetRepository : IPetRepository
{
    private readonly Context _context;

    public PetRepository(Context context)
    {
        _context = context;
    }

    public async Task SalvarAsync(Pet pet)
    {
        await _context.Pets.AddAsync(pet);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Pet pet)
    {
        _context.Pets.Update(pet);
        await _context.SaveChangesAsync();
    }

    public Task<Pet?> BuscarPorIdAsync(Guid id)
    {
        return _context.Pets.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Pet>> BuscarPorDonoIdAsync(Guid donoId)
    {
        return await _context.Pets.Where(p => p.DonoId == donoId).ToListAsync();
    }
    
    public async Task<bool> ExistePorIdAsync(Guid id, Guid ownerId)
    {
        return await _context.Pets.AnyAsync(p => p.Id == id && p.DonoId == ownerId);
    }
}