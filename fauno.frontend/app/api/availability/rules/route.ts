import { NextResponse } from "next/server";
import { backend, BackendError } from "@/lib/backend";
import { getSession } from "@/lib/session";
import type { CreateAvailabilityRulePayload } from "@/lib/types";

// Cria uma regra de disponibilidade (vet). O backend deriva o vet do JWT.
export async function POST(req: Request) {
  const session = await getSession();
  if (!session || session.role !== "vet") {
    return NextResponse.json({ error: "Não autorizado." }, { status: 403 });
  }
  try {
    const payload = (await req.json()) as CreateAvailabilityRulePayload;
    await backend("agenda", "AvailabilityRule", {
      method: "POST",
      body: payload,
    });
    return NextResponse.json({ ok: true }, { status: 201 });
  } catch (e) {
    const status = e instanceof BackendError ? e.status : 500;
    return NextResponse.json(
      { error: e instanceof Error ? e.message : "Erro ao criar regra." },
      { status },
    );
  }
}
