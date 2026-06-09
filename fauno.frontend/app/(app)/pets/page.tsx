"use client";

import { useEffect, useState } from "react";
import { toast } from "sonner";
import { PawPrintIcon, PencilSimpleIcon, PlusIcon } from "@phosphor-icons/react";
import { api, ApiError } from "@/lib/api-client";
import type { Pet } from "@/lib/types";
import PageHeader from "@/components/header";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent } from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";

type FormState = { nome: string; especie: string; raca: string };
const EMPTY: FormState = { nome: "", especie: "", raca: "" };

export default function PetsPage() {
  const [pets, setPets] = useState<Pet[]>([]);
  const [loading, setLoading] = useState(true);
  const [open, setOpen] = useState(false);
  const [editing, setEditing] = useState<Pet | null>(null);
  const [form, setForm] = useState<FormState>(EMPTY);
  const [saving, setSaving] = useState(false);

  async function load() {
    setLoading(true);
    try {
      setPets(await api.get<Pet[]>("/pets"));
    } catch (err) {
      toast.error(err instanceof ApiError ? err.message : "Erro ao carregar.");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    load();
  }, []);

  function openNew() {
    setEditing(null);
    setForm(EMPTY);
    setOpen(true);
  }

  function openEdit(pet: Pet) {
    setEditing(pet);
    setForm({ nome: pet.nome, especie: pet.especie, raca: pet.raca });
    setOpen(true);
  }

  async function save(e: React.FormEvent) {
    e.preventDefault();
    setSaving(true);
    try {
      if (editing) {
        await api.put(`/pets/${editing.id}`, form);
        toast.success("Pet atualizado.");
      } else {
        await api.post("/pets", form);
        toast.success("Pet cadastrado.");
      }
      setOpen(false);
      await load();
    } catch (err) {
      toast.error(err instanceof ApiError ? err.message : "Erro ao salvar.");
    } finally {
      setSaving(false);
    }
  }

  const upd = (k: keyof FormState) => (e: React.ChangeEvent<HTMLInputElement>) =>
    setForm((s) => ({ ...s, [k]: e.target.value }));

  return (
    <>
      <PageHeader
        title="Meus Pets"
        description="Cadastre os pets que vão às consultas."
        action={
          <Button onClick={openNew}>
            <PlusIcon className="size-4" /> Novo pet
          </Button>
        }
      />

      <div className="p-4 sm:p-6">
        {loading ? (
          <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
            {[0, 1, 2].map((i) => (
              <Skeleton key={i} className="h-28 w-full" />
            ))}
          </div>
        ) : pets.length === 0 ? (
          <EmptyState onAdd={openNew} />
        ) : (
          <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
            {pets.map((pet) => (
              <Card key={pet.id}>
                <CardContent className="flex items-start gap-3">
                  <div className="bg-muted rounded-lg p-2.5">
                    <PawPrintIcon weight="fill" className="size-6" />
                  </div>
                  <div className="min-w-0 flex-1">
                    <p className="truncate text-base font-semibold">
                      {pet.nome}
                    </p>
                    <p className="text-muted-foreground text-sm">
                      {pet.especie} · {pet.raca}
                    </p>
                  </div>
                  <Button
                    variant="ghost"
                    size="icon"
                    onClick={() => openEdit(pet)}
                    aria-label="Editar"
                  >
                    <PencilSimpleIcon className="size-4" />
                  </Button>
                </CardContent>
              </Card>
            ))}
          </div>
        )}
      </div>

      <Dialog open={open} onOpenChange={setOpen}>
        <DialogContent>
          <form onSubmit={save}>
            <DialogHeader>
              <DialogTitle>
                {editing ? "Editar pet" : "Novo pet"}
              </DialogTitle>
              <DialogDescription>
                Informe os dados do animal.
              </DialogDescription>
            </DialogHeader>

            <div className="space-y-4 py-4">
              <div className="space-y-2">
                <Label htmlFor="nome">Nome</Label>
                <Input
                  id="nome"
                  required
                  value={form.nome}
                  onChange={upd("nome")}
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="especie">Espécie</Label>
                <Input
                  id="especie"
                  required
                  placeholder="Cão, Gato..."
                  value={form.especie}
                  onChange={upd("especie")}
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="raca">Raça</Label>
                <Input
                  id="raca"
                  required
                  value={form.raca}
                  onChange={upd("raca")}
                />
              </div>
            </div>

            <DialogFooter>
              <Button type="submit" disabled={saving}>
                {saving ? "Salvando..." : "Salvar"}
              </Button>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>
    </>
  );
}

function EmptyState({ onAdd }: { onAdd: () => void }) {
  return (
    <div className="flex flex-col items-center justify-center gap-3 rounded-xl border border-dashed py-16 text-center">
      <PawPrintIcon className="text-muted-foreground size-10" />
      <div>
        <p className="font-medium">Nenhum pet cadastrado</p>
        <p className="text-muted-foreground text-sm">
          Cadastre um pet para conseguir agendar consultas.
        </p>
      </div>
      <Button onClick={onAdd}>
        <PlusIcon className="size-4" /> Cadastrar pet
      </Button>
    </div>
  );
}
