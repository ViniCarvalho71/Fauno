"use client";

import { useEffect, useState } from "react";
import { toast } from "sonner";
import {
  CalendarBlankIcon,
  CheckCircleIcon,
} from "@phosphor-icons/react";
import { api, ApiError } from "@/lib/api-client";
import {
  AppointmentType,
  APPOINTMENT_TYPE_LABEL,
  type AvailableSlot,
  type Pet,
  type VetProfile,
} from "@/lib/types";
import { combineDateTime, toIsoDate, trimSeconds, formatLongDate } from "@/lib/format";
import PageHeader from "@/components/header";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Calendar } from "@/components/ui/calendar";
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";

export default function AgendarPage() {
  const [pets, setPets] = useState<Pet[]>([]);
  const [vets, setVets] = useState<VetProfile[]>([]);
  const [vetId, setVetId] = useState("");
  const [vet, setVet] = useState<VetProfile | null>(null);

  const [date, setDate] = useState<Date | undefined>();
  const [slots, setSlots] = useState<AvailableSlot[]>([]);
  const [slotsLoading, setSlotsLoading] = useState(false);
  const [slot, setSlot] = useState<AvailableSlot | null>(null);

  const [petId, setPetId] = useState("");
  const [type, setType] = useState<string>(String(AppointmentType.Consultation));
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    api
      .get<Pet[]>("/pets")
      .then(setPets)
      .catch(() => setPets([]));
    api
      .get<VetProfile[]>("/vets")
      .then(setVets)
      .catch(() => {
        setVets([]);
        toast.error("Erro ao carregar veterinários.");
      });
  }, []);

  function pickVet(id: string) {
    setVetId(id);
    setDate(undefined);
    setSlots([]);
    setSlot(null);
    setVet(vets.find((v) => v.id === id) ?? null);
  }

  async function loadSlots(d: Date) {
    if (!vet) return;
    setSlotsLoading(true);
    setSlot(null);
    try {
      const res = await api.get<AvailableSlot[]>(
        `/availability/slots?veterinarianId=${vet.id}&date=${toIsoDate(d)}`,
      );
      setSlots(res);
    } catch (err) {
      toast.error(err instanceof ApiError ? err.message : "Erro ao buscar horários.");
      setSlots([]);
    } finally {
      setSlotsLoading(false);
    }
  }

  function pickDate(d: Date | undefined) {
    setDate(d);
    setSlots([]);
    setSlot(null);
    if (d) loadSlots(d);
  }

  async function submit(e: React.FormEvent) {
    e.preventDefault();
    if (!vet || !date || !slot || !petId) {
      toast.error("Selecione veterinário, data, horário e pet.");
      return;
    }
    setSubmitting(true);
    try {
      const isoDate = toIsoDate(date);
      await api.post("/appointments", {
        veterinarianId: vet.id,
        petId,
        title: title || null,
        description: description || null,
        start: combineDateTime(isoDate, slot.start),
        end: combineDateTime(isoDate, slot.end),
        appointmentType: Number(type),
      });
      toast.success("Consulta agendada!");
      // reseta horario, mantem vet/data pra agendar outro
      setSlot(null);
      setTitle("");
      setDescription("");
      await loadSlots(date);
    } catch (err) {
      toast.error(err instanceof ApiError ? err.message : "Erro ao agendar.");
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <>
      <PageHeader
        title="Agendar consulta"
        description="Escolha o veterinário, o dia e um horário livre."
      />

      <div className="grid gap-4 p-4 sm:p-6 lg:grid-cols-2">
        {/* 1. Veterinário */}
        <Card>
          <CardHeader>
            <CardTitle className="text-base">1. Veterinário</CardTitle>
          </CardHeader>
          <CardContent className="space-y-3">
            <Label htmlFor="vetId">Veterinário</Label>
            <Select value={vetId} onValueChange={pickVet}>
              <SelectTrigger id="vetId">
                <SelectValue placeholder="Selecione o veterinário" />
              </SelectTrigger>
              <SelectContent>
                {vets.length === 0 ? (
                  <div className="text-muted-foreground p-2 text-sm">
                    Nenhum veterinário disponível.
                  </div>
                ) : (
                  vets.map((v) => (
                    <SelectItem key={v.id} value={v.id}>
                      {v.nome} · CRMV {v.crmv}
                    </SelectItem>
                  ))
                )}
              </SelectContent>
            </Select>
            {vet && (
              <div className="bg-muted flex items-center gap-2 rounded-lg p-3 text-sm">
                <CheckCircleIcon weight="fill" className="size-5 text-emerald-600" />
                <span>
                  <strong>{vet.nome}</strong> · CRMV {vet.crmv}
                </span>
              </div>
            )}
          </CardContent>
        </Card>

        {/* 2. Data */}
        <Card>
          <CardHeader>
            <CardTitle className="text-base">2. Data</CardTitle>
          </CardHeader>
          <CardContent>
            <Popover>
              <PopoverTrigger asChild>
                <Button
                  variant="outline"
                  className="w-full justify-start font-normal"
                  disabled={!vet}
                >
                  <CalendarBlankIcon className="size-4" />
                  {date ? formatLongDate(toIsoDate(date)) : "Escolher dia"}
                </Button>
              </PopoverTrigger>
              <PopoverContent className="w-auto p-0" align="start">
                <Calendar
                  mode="single"
                  selected={date}
                  onSelect={pickDate}
                  disabled={(d) =>
                    d < new Date(new Date().setHours(0, 0, 0, 0))
                  }
                />
              </PopoverContent>
            </Popover>
          </CardContent>
        </Card>

        {/* 3. Horários */}
        <Card className="lg:col-span-2">
          <CardHeader>
            <CardTitle className="text-base">3. Horário</CardTitle>
          </CardHeader>
          <CardContent>
            {!vet || !date ? (
              <p className="text-muted-foreground text-sm">
                Selecione veterinário e data para ver os horários.
              </p>
            ) : slotsLoading ? (
              <p className="text-muted-foreground text-sm">Carregando...</p>
            ) : slots.length === 0 ? (
              <p className="text-muted-foreground text-sm">
                Nenhum horário disponível nesse dia.
              </p>
            ) : (
              <div className="flex flex-wrap gap-2">
                {slots.map((s) => {
                  const selected = slot?.start === s.start;
                  return (
                    <button
                      key={`${s.start}-${s.end}`}
                      type="button"
                      onClick={() => setSlot(s)}
                      className={
                        "rounded-lg border px-3 py-2 text-sm font-medium transition-colors " +
                        (selected
                          ? "border-primary bg-primary text-primary-foreground"
                          : "hover:bg-accent")
                      }
                    >
                      {trimSeconds(s.start)} – {trimSeconds(s.end)}
                    </button>
                  );
                })}
              </div>
            )}
          </CardContent>
        </Card>

        {/* 4. Detalhes + confirmar */}
        <Card className="lg:col-span-2">
          <CardHeader>
            <CardTitle className="text-base">4. Detalhes</CardTitle>
          </CardHeader>
          <CardContent>
            <form onSubmit={submit} className="space-y-4">
              <div className="grid gap-4 sm:grid-cols-2">
                <div className="space-y-2">
                  <Label>Pet</Label>
                  <Select value={petId} onValueChange={setPetId}>
                    <SelectTrigger>
                      <SelectValue placeholder="Selecione o pet" />
                    </SelectTrigger>
                    <SelectContent>
                      {pets.length === 0 ? (
                        <div className="text-muted-foreground p-2 text-sm">
                          Cadastre um pet primeiro.
                        </div>
                      ) : (
                        pets.map((p) => (
                          <SelectItem key={p.id} value={p.id}>
                            {p.nome} ({p.especie})
                          </SelectItem>
                        ))
                      )}
                    </SelectContent>
                  </Select>
                </div>
                <div className="space-y-2">
                  <Label>Tipo</Label>
                  <Select value={type} onValueChange={setType}>
                    <SelectTrigger>
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      {Object.values(AppointmentType)
                        .filter((v) => typeof v === "number")
                        .map((v) => (
                          <SelectItem key={v} value={String(v)}>
                            {APPOINTMENT_TYPE_LABEL[v as AppointmentType]}
                          </SelectItem>
                        ))}
                    </SelectContent>
                  </Select>
                </div>
              </div>

              <div className="space-y-2">
                <Label htmlFor="title">Título (opcional)</Label>
                <Input
                  id="title"
                  value={title}
                  onChange={(e) => setTitle(e.target.value)}
                  placeholder="Ex.: Consulta de rotina"
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="desc">Descrição (opcional)</Label>
                <Textarea
                  id="desc"
                  value={description}
                  onChange={(e) => setDescription(e.target.value)}
                  placeholder="Sintomas, observações..."
                />
              </div>

              <div className="flex items-center justify-between gap-4">
                <p className="text-muted-foreground text-sm">
                  {slot
                    ? `Horário: ${trimSeconds(slot.start)} – ${trimSeconds(slot.end)}`
                    : "Nenhum horário selecionado."}
                </p>
                <Button type="submit" disabled={submitting || !slot || !petId}>
                  {submitting ? "Agendando..." : "Confirmar agendamento"}
                </Button>
              </div>
            </form>
          </CardContent>
        </Card>
      </div>
    </>
  );
}
