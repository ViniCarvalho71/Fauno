using Fauno.Agenda.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Agenda.Application.DTOs
{
    public class RecurrenceDto
    {
        public RecurrenceMode Mode { get; set; }

        // Weekly
        public List<DayOfWeek>? DaysOfWeek { get; set; }
        public DateOnly? PeriodStart { get; set; }
        public DateOnly? PeriodEnd { get; set; }

        // SpecificDates
        public List<DateOnly>? Dates { get; set; }
    }
}
