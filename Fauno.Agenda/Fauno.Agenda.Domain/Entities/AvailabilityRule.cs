using Fauno.Agenda.Domain.Exceptions;
using Fauno.Agenda.Domain.ValueObjects;

namespace Fauno.Agenda.Domain.Entities
{
    public class AvailabilityRule : EntityBase
    {
        public Guid VeterinarianId { get; private set; }
        public TimeOnly SlotStart { get; private set; }
        public TimeOnly SlotEnd { get; private set; }
        public int SlotDurationMinutes { get; private set; }
        public PauseWindow? Pause { get; private set; }
        public Recurrence Recurrence { get; private set; } = null!;

        protected AvailabilityRule() { }

        public AvailabilityRule(
            Guid veterinarianId,
            TimeOnly slotStart,
            TimeOnly slotEnd,
            int slotDurationMinutes,
            Recurrence recurrence,
            PauseWindow? pause = null)
        {
            if (slotEnd <= slotStart)
                throw new DomainException("O horário fim deve ser após o início.");
            if (slotDurationMinutes <= 0)
                throw new DomainException("A duração do slot deve ser positiva.");

            VeterinarianId = veterinarianId;
            SlotStart = slotStart;
            SlotEnd = slotEnd;
            SlotDurationMinutes = slotDurationMinutes;
            Recurrence = recurrence;
            Pause = pause;
        }

        // Gera todos os slots de um dia específico
        public IEnumerable<(TimeOnly Start, TimeOnly End)> GenerateSlotsFor(DateOnly date)
        {
            // Verifica se essa regra cobre essa data
            if (!Recurrence.ResolveDates().Contains(date))
                yield break;

            var current = SlotStart;
            var slotSpan = TimeSpan.FromMinutes(SlotDurationMinutes);

            while (current.Add(slotSpan) <= SlotEnd)
            {
                var next = current.Add(slotSpan);

                // Pula slots que colidem com a pausa
                bool collidesWithPause = Pause is not null &&
                    current < Pause.End && next > Pause.Start;

                if (!collidesWithPause)
                    yield return (current, next);

                current = next;
            }
        }
    }
}