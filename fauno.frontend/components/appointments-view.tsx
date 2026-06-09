"use client";

import { useCallback, useEffect, useState } from "react";
import { toast } from "sonner";
import { CalendarBlankIcon, FunnelXIcon } from "@phosphor-icons/react";
import { api, ApiError } from "@/lib/api-client";
import type { AppointmentResponse, Role } from "@/lib/types";
import { toIsoDate, formatLongDate } from "@/lib/format";
import PageHeader from "@/components/header";
import { AppointmentCard } from "@/components/appointment-card";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { Calendar } from "@/components/ui/calendar";
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover";

const ORDER = { Scheduled: 0, Confirmed: 1, Finished: 2, Cancelled: 3 } as const;

export function AppointmentsView({
  role,
  title,
  description,
}: {
  role: Role;
  title: string;
  description: string;
}) {
  const [items, setItems] = useState<AppointmentResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [busyId, setBusyId] = useState<string | null>(null);
  const [date, setDate] = useState<Date | undefined>();

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const qs = date ? `?date=${toIsoDate(date)}` : "";
      const list = await api.get<AppointmentResponse[]>(`/appointments${qs}`);
      list.sort((a, b) => {
        const s =
          (ORDER[a.status as keyof typeof ORDER] ?? 9) -
          (ORDER[b.status as keyof typeof ORDER] ?? 9);
        if (s !== 0) return s;
        return a.start.localeCompare(b.start);
      });
      setItems(list);
    } catch (err) {
      toast.error(err instanceof ApiError ? err.message : "Erro ao carregar.");
    } finally {
      setLoading(false);
    }
  }, [date]);

  useEffect(() => {
    load();
  }, [load]);

  async function act(
    id: string,
    action: "cancel" | "confirm" | "finish",
  ) {
    setBusyId(id);
    try {
      await api.patch(`/appointments/${id}/${action}`);
      toast.success("Atualizado.");
      await load();
    } catch (err) {
      toast.error(err instanceof ApiError ? err.message : "Falha na operação.");
    } finally {
      setBusyId(null);
    }
  }

  return (
    <>
      <PageHeader
        title={title}
        description={description}
        action={
          <div className="flex items-center gap-2">
            {date && (
              <Button
                variant="ghost"
                size="icon"
                onClick={() => setDate(undefined)}
                aria-label="Limpar filtro"
              >
                <FunnelXIcon className="size-4" />
              </Button>
            )}
            <Popover>
              <PopoverTrigger asChild>
                <Button variant="outline" className="font-normal">
                  <CalendarBlankIcon className="size-4" />
                  {date ? formatLongDate(toIsoDate(date)) : "Filtrar por dia"}
                </Button>
              </PopoverTrigger>
              <PopoverContent className="w-auto p-0" align="end">
                <Calendar mode="single" selected={date} onSelect={setDate} />
              </PopoverContent>
            </Popover>
          </div>
        }
      />

      <div className="p-4 sm:p-6">
        {loading ? (
          <div className="grid gap-4 md:grid-cols-2">
            {[0, 1, 2, 3].map((i) => (
              <Skeleton key={i} className="h-40 w-full" />
            ))}
          </div>
        ) : items.length === 0 ? (
          <div className="flex flex-col items-center justify-center gap-2 rounded-xl border border-dashed py-16 text-center">
            <CalendarBlankIcon className="text-muted-foreground size-10" />
            <p className="font-medium">Nenhuma consulta encontrada</p>
            <p className="text-muted-foreground text-sm">
              {date
                ? "Não há consultas nesse dia."
                : role === "owner"
                  ? "Agende uma consulta para começar."
                  : "As consultas marcadas com você aparecerão aqui."}
            </p>
          </div>
        ) : (
          <div className="grid gap-4 md:grid-cols-2">
            {items.map((appt) => (
              <AppointmentCard
                key={appt.id}
                appt={appt}
                role={role}
                busy={busyId === appt.id}
                onAction={act}
              />
            ))}
          </div>
        )}
      </div>
    </>
  );
}
