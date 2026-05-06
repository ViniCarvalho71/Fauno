using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Auth.Domain.Entities
{
    public class EntityBase
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
