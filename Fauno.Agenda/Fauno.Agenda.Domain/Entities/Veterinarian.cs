using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Agenda.Domain.Entities
{
    public class Veterinarian : EntityBase
    {
        public DateTime Start {  get; private set; }
        public DateTime Period { get; private set; }

    }
}
