using System.Text.Json.Serialization;
using Fauno.Agenda.Domain.Enums;
using Fauno.Agenda.Domain.Exceptions;

namespace Fauno.Agenda.Domain.ValueObjects
{
    public class Recurrence
    {
        [JsonInclude]
        public RecurrenceMode Mode { get; private set; }

        // Weekly
        [JsonInclude]
        public IReadOnlyList<DayOfWeek>? DaysOfWeek { get; private set; }

        [JsonInclude]
        public DateOnly? PeriodStart { get; private set; }

        [JsonInclude]
        public DateOnly? PeriodEnd { get; private set; }

        // SpecificDates
        [JsonInclude]
        public IReadOnlyList<DateOnly>? Dates { get; private set; }

        [JsonConstructor]
        protected Recurrence() { }

        public static Recurrence Weekly(
            IEnumerable<DayOfWeek> days,
            DateOnly periodStart,
            DateOnly periodEnd)
        {
            if (!days.Any())
                throw new DomainException("Informe ao menos um dia da semana.");
            if (periodEnd <= periodStart)
                throw new DomainException("O período fim deve ser após o início.");

            return new Recurrence
            {
                Mode = RecurrenceMode.Weekly,
                DaysOfWeek = days.Distinct().ToList(),
                PeriodStart = periodStart,
                PeriodEnd = periodEnd
            };
        }

        public static Recurrence SpecificDates(IEnumerable<DateOnly> dates)
        {
            var list = dates.Distinct().OrderBy(d => d).ToList();

            if (!list.Any())
                throw new DomainException("Informe ao menos uma data.");

            return new Recurrence
            {
                Mode = RecurrenceMode.SpecificDates,
                Dates = list
            };
        }

        // Retorna todas as datas que essa recorrência cobre
        public IEnumerable<DateOnly> ResolveDates()
        {
            if (Mode == RecurrenceMode.SpecificDates)
                return Dates!;

            var result = new List<DateOnly>();
            var current = PeriodStart!.Value;

            while (current <= PeriodEnd!.Value)
            {
                if (DaysOfWeek!.Contains(current.DayOfWeek))
                    result.Add(current);

                current = current.AddDays(1);
            }

            return result;
        }
    }
}