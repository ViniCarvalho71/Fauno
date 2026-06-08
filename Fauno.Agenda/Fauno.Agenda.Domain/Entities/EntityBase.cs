using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Fauno.Agenda.Domain.Entities
{
    public abstract class EntityBase
    {
        public Guid Id { get; private set; }
        public DateTime CreatedAt { get; private set; } 

        public DateTime? RemovedAt { get; private set; }

        public EntityBase()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
        public void Remove() 
        {
            RemovedAt = DateTime.UtcNow;
        }
    }
}
