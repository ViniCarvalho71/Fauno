using Fauno.Agenda.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Agenda.Application.DTOs
{
    public class RecurrenceDto
    {
        public RecurrenceMode Mode { get; set; }

        public List<DayOfWeek>? DaysOfWeek { get; set; }
        public DateOnly? PeriodStart { get; set; }
        public DateOnly? PeriodEnd { get; set; }

        public List<DateOnly>? Dates { get; set; }
    }
}
