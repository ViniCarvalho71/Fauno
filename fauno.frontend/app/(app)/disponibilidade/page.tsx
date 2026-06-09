"use client";

import { useState } from "react";
import { toast } from "sonner";
import { InfoIcon, PlusIcon, XIcon, ProhibitIcon } from "@phosphor-icons/react";
import { api, ApiError } from "@/lib/api-client";
import {
  RecurrenceMode,
  WEEKDAYS,
  type CreateAvailabilityRulePayload,
} from "@/lib/types";
import { formatLongDate } from "@/lib/format";
import PageHeader from "@/components/header";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Checkbox } from "@/components/ui/checkbox";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";

export default function DisponibilidadePage() {
  return (
    <>
      <PageHeader
        title="Disponibilidade"
        description="Defina os horários que você atende e bloqueie dias específicos."
      />
      <div className="grid gap-4 p-4 sm:p-6 lg:grid-cols-2">
        <RuleForm />
        <div className="space-y-4">
          <ExceptionForm />
          <Note />
        </div>
      </div>
    </>
  );
}

function RuleForm() {
  const [slotStart, setSlotStart] = useState("08:00");
  const [slotEnd, setSlotEnd] = useState("12:00");
  const [duration, setDuration] = useState(30);

  const [pauseOn, setPauseOn] = useState(false);
  const [pauseStart, setPauseStart] = useState("10:00");
  const [pauseEnd, setPauseEnd] = useState("10:30");

  const [mode, setMode] = useState<RecurrenceMode>(RecurrenceMode.Weekly);
  const [days, setDays] = useState<number[]>([1, 2, 3, 4, 5]);
  const [periodStart, setPeriodStart] = useState("");
  const [periodEnd, setPeriodEnd] = useState("");

  const [dates, setDates] = useState<string[]>([]);
  const [dateToAdd, setDateToAdd] = useState("");

  const [saving, setSaving] = useState(false);

  function toggleDay(v: number) {
    setDays((d) => (d.includes(v) ? d.filter((x) => x !== v) : [...d, v]));
  }

  function addDate() {
    if (dateToAdd && !dates.includes(dateToAdd)) {
      setDates((d) => [...d, dateToAdd].sort());
    }
    setDateToAdd("");
  }

  async function submit(e: React.FormEvent) {
    e.preventDefault();

    if (mode === RecurrenceMode.Weekly && days.length === 0) {
      toast.error("Selecione ao menos um dia da semana.");
      return;
    }
    if (mode === RecurrenceMode.SpecificDates && dates.length === 0) {
      toast.error("Adicione ao menos uma data.");
      return;
    }

    const payload: CreateAvailabilityRulePayload = {
      slotStart,
      slotEnd,
      slotDurationMinutes: Number(duration),
      pause: pauseOn ? { start: pauseStart, end: pauseEnd } : null,
      recurrence:
        mode === RecurrenceMode.Weekly
          ? {
              mode,
              daysOfWeek: days,
              periodStart: periodStart || null,
              periodEnd: periodEnd || null,
              dates: null,
            }
          : {
              mode,
              daysOfWeek: null,
              periodStart: null,
              periodEnd: null,
              dates,
            },
    };

    setSaving(true);
    try {
      await api.post("/availability/rules", payload);
      toast.success("Regra de disponibilidade criada.");
    } catch (err) {
      toast.error(err instanceof ApiError ? err.message : "Erro ao criar regra.");
    } finally {
      setSaving(false);
    }
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle className="text-base">Nova regra de horário</CardTitle>
        <CardDescription>
          O sistema gera os slots automaticamente dentro da janela.
        </CardDescription>
      </CardHeader>
      <CardContent>
        <form onSubmit={submit} className="space-y-5">
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="ss">Início</Label>
              <Input
                id="ss"
                type="time"
                value={slotStart}
                onChange={(e) => setSlotStart(e.target.value)}
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="se">Fim</Label>
              <Input
                id="se"
                type="time"
                value={slotEnd}
                onChange={(e) => setSlotEnd(e.target.value)}
              />
            </div>
          </div>

          <div className="space-y-2">
            <Label htmlFor="dur">Duração de cada slot (minutos)</Label>
            <Input
              id="dur"
              type="number"
              min={1}
              value={duration}
              onChange={(e) => setDuration(Number(e.target.value))}
            />
          </div>

          <div className="space-y-3 rounded-lg border p-3">
            <label className="flex items-center gap-2 text-sm font-medium">
              <Checkbox
                checked={pauseOn}
                onCheckedChange={(c) => setPauseOn(Boolean(c))}
              />
              Intervalo (pausa)
            </label>
            {pauseOn && (
              <div className="grid grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label htmlFor="ps">Início da pausa</Label>
                  <Input
                    id="ps"
                    type="time"
                    value={pauseStart}
                    onChange={(e) => setPauseStart(e.target.value)}
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="pe">Fim da pausa</Label>
                  <Input
                    id="pe"
                    type="time"
                    value={pauseEnd}
                    onChange={(e) => setPauseEnd(e.target.value)}
                  />
                </div>
              </div>
            )}
          </div>

          <div className="space-y-3">
            <Label>Repetição</Label>
            <Tabs
              value={String(mode)}
              onValueChange={(v) => setMode(Number(v) as RecurrenceMode)}
            >
              <TabsList className="grid w-full grid-cols-2">
                <TabsTrigger value={String(RecurrenceMode.Weekly)}>
                  Semanal
                </TabsTrigger>
                <TabsTrigger value={String(RecurrenceMode.SpecificDates)}>
                  Datas específicas
                </TabsTrigger>
              </TabsList>

              <TabsContent value={String(RecurrenceMode.Weekly)} className="space-y-4 pt-3">
                <div className="flex flex-wrap gap-2">
                  {WEEKDAYS.map((d) => {
                    const on = days.includes(d.value);
                    return (
                      <button
                        type="button"
                        key={d.value}
                        onClick={() => toggleDay(d.value)}
                        className={
                          "rounded-md border px-3 py-1.5 text-sm transition-colors " +
                          (on
                            ? "border-primary bg-primary text-primary-foreground"
                            : "hover:bg-accent")
                        }
                      >
                        {d.short}
                      </button>
                    );
                  })}
                </div>
                <div className="grid grid-cols-2 gap-4">
                  <div className="space-y-2">
                    <Label htmlFor="prs">Início do período (opcional)</Label>
                    <Input
                      id="prs"
                      type="date"
                      value={periodStart}
                      onChange={(e) => setPeriodStart(e.target.value)}
                    />
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="pre">Fim do período (opcional)</Label>
                    <Input
                      id="pre"
                      type="date"
                      value={periodEnd}
                      onChange={(e) => setPeriodEnd(e.target.value)}
                    />
                  </div>
                </div>
              </TabsContent>

              <TabsContent value={String(RecurrenceMode.SpecificDates)} className="space-y-3 pt-3">
                <div className="flex gap-2">
                  <Input
                    type="date"
                    value={dateToAdd}
                    onChange={(e) => setDateToAdd(e.target.value)}
                  />
                  <Button type="button" variant="secondary" onClick={addDate}>
                    <PlusIcon className="size-4" /> Adicionar
                  </Button>
                </div>
                {dates.length > 0 && (
                  <div className="flex flex-wrap gap-2">
                    {dates.map((d) => (
                      <span
                        key={d}
                        className="bg-muted flex items-center gap-1 rounded-md px-2 py-1 text-sm"
                      >
                        {formatLongDate(d)}
                        <button
                          type="button"
                          onClick={() =>
                            setDates((s) => s.filter((x) => x !== d))
                          }
                          aria-label="Remover"
                        >
                          <XIcon className="size-3.5" />
                        </button>
                      </span>
                    ))}
                  </div>
                )}
              </TabsContent>
            </Tabs>
          </div>

          <Button type="submit" className="w-full" disabled={saving}>
            {saving ? "Salvando..." : "Criar regra"}
          </Button>
        </form>
      </CardContent>
    </Card>
  );
}

function ExceptionForm() {
  const [date, setDate] = useState("");
  const [reason, setReason] = useState("");
  const [saving, setSaving] = useState(false);

  async function submit(e: React.FormEvent) {
    e.preventDefault();
    if (!date) {
      toast.error("Escolha a data a bloquear.");
      return;
    }
    setSaving(true);
    try {
      await api.post("/availability/exceptions", {
        date,
        reason: reason || null,
      });
      toast.success("Dia bloqueado.");
      setReason("");
      setDate("");
    } catch (err) {
      toast.error(err instanceof ApiError ? err.message : "Erro ao bloquear.");
    } finally {
      setSaving(false);
    }
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle className="text-base">Bloquear um dia</CardTitle>
        <CardDescription>
          Marque folgas ou ausências. Nenhum horário fica disponível no dia.
        </CardDescription>
      </CardHeader>
      <CardContent>
        <form onSubmit={submit} className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="ex-date">Data</Label>
            <Input
              id="ex-date"
              type="date"
              value={date}
              onChange={(e) => setDate(e.target.value)}
            />
          </div>
          <div className="space-y-2">
            <Label htmlFor="ex-reason">Motivo (opcional)</Label>
            <Input
              id="ex-reason"
              value={reason}
              onChange={(e) => setReason(e.target.value)}
              placeholder="Ex.: Feriado, congresso..."
            />
          </div>
          <Button type="submit" variant="destructive" className="w-full" disabled={saving}>
            <ProhibitIcon className="size-4" />
            {saving ? "Bloqueando..." : "Bloquear dia"}
          </Button>
        </form>
      </CardContent>
    </Card>
  );
}

function Note() {
  return (
    <div className="text-muted-foreground flex gap-2 rounded-lg border border-dashed p-3 text-sm">
      <InfoIcon className="mt-0.5 size-4 shrink-0" />
      <p>
        As APIs ainda não permitem listar as regras já criadas — por isso esta
        tela apenas adiciona novas regras e bloqueios.
      </p>
    </div>
  );
}
