import { NextResponse } from "next/server";
import { backend, BackendError } from "@/lib/backend";
import { getSession } from "@/lib/session";

const ALLOWED = new Set(["cancel", "confirm", "finish"]);

// Transicoes de estado da consulta. cancel: dono ou vet; confirm/finish: vet.
export async function PATCH(
  _req: Request,
  { params }: { params: Promise<{ id: string; action: string }> },
) {
  const session = await getSession();
  if (!session) {
    return NextResponse.json({ error: "Não autenticado." }, { status: 401 });
  }
  const { id, action } = await params;
  if (!ALLOWED.has(action)) {
    return NextResponse.json({ error: "Ação inválida." }, { status: 400 });
  }
  if (action !== "cancel" && session.role !== "vet") {
    return NextResponse.json({ error: "Não autorizado." }, { status: 403 });
  }
  try {
    await backend("agenda", `Appointment/${id}/${action}`, {
      method: "PATCH",
    });
    return NextResponse.json({ ok: true });
  } catch (e) {
    const status = e instanceof BackendError ? e.status : 500;
    return NextResponse.json(
      { error: e instanceof Error ? e.message : "Erro na operação." },
      { status },
    );
  }
}
