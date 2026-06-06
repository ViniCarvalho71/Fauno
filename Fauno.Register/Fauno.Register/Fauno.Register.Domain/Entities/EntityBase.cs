using System;

namespace Fauno.Register.Domain.Entities;

public abstract class EntityBase
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; } 
    public DateTime? RemovedAt { get; private set; }

    protected EntityBase()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public void Remove() 
    {
        RemovedAt = DateTime.UtcNow;
    }
}