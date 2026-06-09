import { NextResponse } from "next/server";
import { backend, BackendError } from "@/lib/backend";
import { getSession } from "@/lib/session";
import type { AppointmentResponse, MakeAppointmentPayload } from "@/lib/types";

// Lista consultas do usuario logado (dono ou vet), opcionalmente por data.
export async function GET(req: Request) {
  const session = await getSession();
  if (!session) {
    return NextResponse.json({ error: "Não autenticado." }, { status: 401 });
  }
  const date = new URL(req.url).searchParams.get("date") ?? undefined;
  const path =
    session.role === "owner"
      ? "Appointment/owner"
      : "Appointment/veterinarian";
  try {
    const list = await backend<AppointmentResponse[]>("agenda", path, {
      query: { date },
    });
    return NextResponse.json(list);
  } catch (e) {
    const status = e instanceof BackendError ? e.status : 500;
    return NextResponse.json(
      { error: e instanceof Error ? e.message : "Erro ao listar consultas." },
      { status },
    );
  }
}

// Agenda uma consulta (dono).
export async function POST(req: Request) {
  const session = await getSession();
  if (!session || session.role !== "owner") {
    return NextResponse.json({ error: "Não autorizado." }, { status: 403 });
  }
  try {
    const payload = (await req.json()) as MakeAppointmentPayload;
    await backend("agenda", "Appointment", {
      method: "POST",
      body: payload,
    });
    return NextResponse.json({ ok: true }, { status: 201 });
  } catch (e) {
    const status = e instanceof BackendError ? e.status : 500;
    return NextResponse.json(
      { error: e instanceof Error ? e.message : "Erro ao agendar." },
      { status },
    );
  }
}
