import { NextResponse } from "next/server";
import { backend, BackendError } from "@/lib/backend";
import { getSession } from "@/lib/session";
import type { CreateAvailabilityExceptionPayload } from "@/lib/types";

// Bloqueia um dia da agenda do vet (excecao). Vet derivado do JWT no backend.
export async function POST(req: Request) {
  const session = await getSession();
  if (!session || session.role !== "vet") {
    return NextResponse.json({ error: "Não autorizado." }, { status: 403 });
  }
  try {
    const payload = (await req.json()) as CreateAvailabilityExceptionPayload;
    if (!payload.date) {
      return NextResponse.json({ error: "Informe a data." }, { status: 400 });
    }
    await backend("agenda", "AvailabilityException", {
      method: "POST",
      body: payload,
    });
    return NextResponse.json({ ok: true }, { status: 201 });
  } catch (e) {
    const status = e instanceof BackendError ? e.status : 500;
    return NextResponse.json(
      { error: e instanceof Error ? e.message : "Erro ao bloquear dia." },
      { status },
    );
  }
}
