"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import { toast } from "sonner";
import { api, ApiError } from "@/lib/api-client";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import type { Role } from "@/lib/types";

export default function CadastroPage() {
  const router = useRouter();
  const [loading, setLoading] = useState(false);

  async function submit(path: string, body: Record<string, string>) {
    setLoading(true);
    try {
      const { role } = await api.post<{ role: Role }>(path, body);
      toast.success("Conta criada!");
      router.replace(role === "vet" ? "/agenda" : "/agendar");
      router.refresh();
    } catch (err) {
      toast.error(err instanceof ApiError ? err.message : "Falha no cadastro.");
      setLoading(false);
    }
  }

  return (
    <div className="space-y-6">
      <div className="space-y-1">
        <h1 className="text-2xl font-bold">Criar conta</h1>
        <p className="text-muted-foreground text-sm">
          Você é dono de pet ou veterinário?
        </p>
      </div>

      <Tabs defaultValue="owner">
        <TabsList className="grid w-full grid-cols-2">
          <TabsTrigger value="owner">Dono</TabsTrigger>
          <TabsTrigger value="vet">Veterinário</TabsTrigger>
        </TabsList>

        <TabsContent value="owner" className="pt-4">
          <OwnerForm
            loading={loading}
            onSubmit={(b) => submit("/auth/register/owner", b)}
          />
        </TabsContent>
        <TabsContent value="vet" className="pt-4">
          <VetForm
            loading={loading}
            onSubmit={(b) => submit("/auth/register/vet", b)}
          />
        </TabsContent>
      </Tabs>

      <p className="text-muted-foreground text-center text-sm">
        Já tem conta?{" "}
        <Link href="/login" className="text-foreground font-medium underline">
          Entrar
        </Link>
      </p>
    </div>
  );
}

function OwnerForm({
  loading,
  onSubmit,
}: {
  loading: boolean;
  onSubmit: (b: Record<string, string>) => void;
}) {
  const [f, setF] = useState({ nome: "", cpf: "", email: "", password: "" });
  const upd = (k: string) => (e: React.ChangeEvent<HTMLInputElement>) =>
    setF((s) => ({ ...s, [k]: e.target.value }));

  return (
    <form
      className="space-y-4"
      onSubmit={(e) => {
        e.preventDefault();
        onSubmit(f);
      }}
    >
      <Field label="Nome" id="o-nome" value={f.nome} onChange={upd("nome")} />
      <Field
        label="CPF"
        id="o-cpf"
        value={f.cpf}
        onChange={upd("cpf")}
        placeholder="000.000.000-00"
      />
      <Field
        label="Email"
        id="o-email"
        type="email"
        value={f.email}
        onChange={upd("email")}
      />
      <Field
        label="Senha"
        id="o-pass"
        type="password"
        value={f.password}
        onChange={upd("password")}
      />
      <Button type="submit" className="w-full" disabled={loading}>
        {loading ? "Criando..." : "Criar conta de dono"}
      </Button>
    </form>
  );
}

function VetForm({
  loading,
  onSubmit,
}: {
  loading: boolean;
  onSubmit: (b: Record<string, string>) => void;
}) {
  const [f, setF] = useState({
    nome: "",
    cpf: "",
    crmv: "",
    email: "",
    password: "",
  });
  const upd = (k: string) => (e: React.ChangeEvent<HTMLInputElement>) =>
    setF((s) => ({ ...s, [k]: e.target.value }));

  return (
    <form
      className="space-y-4"
      onSubmit={(e) => {
        e.preventDefault();
        onSubmit(f);
      }}
    >
      <Field label="Nome" id="v-nome" value={f.nome} onChange={upd("nome")} />
      <Field
        label="CPF"
        id="v-cpf"
        value={f.cpf}
        onChange={upd("cpf")}
        placeholder="000.000.000-00"
      />
      <Field label="CRMV" id="v-crmv" value={f.crmv} onChange={upd("crmv")} />
      <Field
        label="Email"
        id="v-email"
        type="email"
        value={f.email}
        onChange={upd("email")}
      />
      <Field
        label="Senha"
        id="v-pass"
        type="password"
        value={f.password}
        onChange={upd("password")}
      />
      <Button type="submit" className="w-full" disabled={loading}>
        {loading ? "Criando..." : "Criar conta de veterinário"}
      </Button>
    </form>
  );
}

function Field({
  label,
  id,
  value,
  onChange,
  type = "text",
  placeholder,
}: {
  label: string;
  id: string;
  value: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  type?: string;
  placeholder?: string;
}) {
  return (
    <div className="space-y-2">
      <Label htmlFor={id}>{label}</Label>
      <Input
        id={id}
        type={type}
        required
        value={value}
        onChange={onChange}
        placeholder={placeholder}
      />
    </div>
  );
}
