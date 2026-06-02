using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Agenda.Application.DTOs
{
    public class PauseWindowDto
    {
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
    }
}
