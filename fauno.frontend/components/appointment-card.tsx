"use client";

import {
  PawPrintIcon,
  UserIcon,
  StethoscopeIcon,
} from "@phosphor-icons/react";
import {
  APPOINTMENT_STATUS_LABEL,
  type AppointmentResponse,
  type AppointmentStatus,
  type Role,
} from "@/lib/types";
import { formatDateTime, formatTimeRange } from "@/lib/format";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";

type Action = "cancel" | "confirm" | "finish";

const STATUS_VARIANT: Record<
  AppointmentStatus,
  "default" | "secondary" | "destructive" | "outline"
> = {
  Scheduled: "secondary",
  Confirmed: "default",
  Finished: "outline",
  Cancelled: "destructive",
};

export function AppointmentCard({
  appt,
  role,
  busy,
  onAction,
}: {
  appt: AppointmentResponse;
  role: Role;
  busy: boolean;
  onAction: (id: string, action: Action) => void;
}) {
  const status = appt.status as AppointmentStatus;
  const closed = status === "Cancelled" || status === "Finished";

  const actions: { label: string; action: Action; variant?: "destructive" | "outline" | "default" }[] = [];
  if (role === "vet") {
    if (status === "Scheduled")
      actions.push({ label: "Confirmar", action: "confirm" });
    if (!closed) actions.push({ label: "Finalizar", action: "finish", variant: "outline" });
  }
  if (!closed) actions.push({ label: "Cancelar", action: "cancel", variant: "destructive" });

  // A contraparte exibida depende do papel de quem olha.
  const counterpart =
    role === "owner"
      ? { icon: StethoscopeIcon, label: appt.veterinarianName, hint: "Veterinário" }
      : { icon: UserIcon, label: appt.ownerName, hint: "Dono" };

  return (
    <Card>
      <CardContent className="space-y-3">
        <div className="flex items-start justify-between gap-3">
          <div className="min-w-0">
            <p className="truncate font-semibold">
              {appt.title || "Consulta"}
            </p>
            <p className="text-muted-foreground text-sm">
              {formatDateTime(appt.start)} · {formatTimeRange(appt.start, appt.end)}
            </p>
          </div>
          <Badge variant={STATUS_VARIANT[status]}>
            {APPOINTMENT_STATUS_LABEL[status] ?? appt.status}
          </Badge>
        </div>

        <div className="grid gap-1.5 text-sm sm:grid-cols-2">
          <span className="flex items-center gap-1.5">
            <PawPrintIcon className="size-4" /> {appt.petName}
          </span>
          <span className="flex items-center gap-1.5">
            <counterpart.icon className="size-4" /> {counterpart.label}
          </span>
        </div>

        {appt.description && appt.description !== "Sem descrição" && (
          <p className="text-muted-foreground border-t pt-2 text-sm">
            {appt.description}
          </p>
        )}

        {actions.length > 0 && (
          <div className="flex flex-wrap gap-2 border-t pt-3">
            {actions.map((a) => (
              <Button
                key={a.action}
                size="sm"
                variant={a.variant ?? "default"}
                disabled={busy}
                onClick={() => onAction(appt.id, a.action)}
              >
                {a.label}
              </Button>
            ))}
          </div>
        )}
      </CardContent>
    </Card>
  );
}
