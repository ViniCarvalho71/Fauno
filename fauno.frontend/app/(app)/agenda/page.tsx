import { AppointmentsView } from "@/components/appointments-view";

export default function AgendaPage() {
  return (
    <AppointmentsView
      role="vet"
      title="Minha Agenda"
      description="Confirme, finalize ou cancele as consultas marcadas com você."
    />
  );
}
