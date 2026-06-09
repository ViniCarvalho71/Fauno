import { PawPrintIcon } from "@phosphor-icons/react/dist/ssr";

export default function AuthLayout({
  children,
}: Readonly<{ children: React.ReactNode }>) {
  return (
    <div className="grid min-h-svh lg:grid-cols-[1.1fr_1fr]">
      {/* Painel de marca — lado esquerdo */}
      <div className="bg-sidebar text-sidebar-foreground hidden flex-col justify-between border-r p-10 lg:flex">
        <div className="flex items-center gap-2">
          <PawPrintIcon weight="fill" className="size-7" />
          <span className="text-2xl font-bold tracking-tight">Fauno</span>
        </div>
        <div className="space-y-3">
          <h2 className="text-3xl font-bold leading-tight">
            Agenda da clínica,
            <br />
            sem bagunça.
          </h2>
          <p className="text-muted-foreground max-w-sm text-sm">
            Donos marcam consultas nos horários que o veterinário abre.
            Veterinários controlam a própria agenda.
          </p>
        </div>
        <p className="text-muted-foreground text-xs">
          Sistema de agendamento veterinário.
        </p>
      </div>

      {/* Formulário — lado direito */}
      <div className="flex items-center justify-center p-6">
        <div className="w-full max-w-sm">{children}</div>
      </div>
    </div>
  );
}
