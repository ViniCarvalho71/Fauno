using Fauno.Agenda.Domain.Exceptions;

namespace Fauno.Agenda.Domain.ValueObjects
{
    public class PauseWindow
    {
        public TimeOnly Start { get; private set; }
        public TimeOnly End { get; private set; }

        protected PauseWindow() { }

        public PauseWindow(TimeOnly start, TimeOnly end)
        {
            if (end <= start)
                throw new DomainException("O fim da pausa deve ser após o início.");

            Start = start;
            End = end;
        }

        public bool Contains(TimeOnly time) =>
            time >= Start && time < End;
    }
}