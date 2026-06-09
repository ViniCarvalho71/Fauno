import { NextResponse } from "next/server";
import { backend, BackendError } from "@/lib/backend";
import { getSession } from "@/lib/session";
import type { AvailableSlot } from "@/lib/types";

// Slots livres de um vet numa data. Usado pelo dono ao agendar.
export async function GET(req: Request) {
  const session = await getSession();
  if (!session) {
    return NextResponse.json({ error: "Não autenticado." }, { status: 401 });
  }
  const sp = new URL(req.url).searchParams;
  const veterinarianId = sp.get("veterinarianId") ?? undefined;
  const date = sp.get("date") ?? undefined;
  if (!veterinarianId || !date) {
    return NextResponse.json(
      { error: "Informe veterinarianId e date." },
      { status: 400 },
    );
  }
  try {
    const slots = await backend<AvailableSlot[]>("agenda", "Availability/slots", {
      query: { VeterinarianId: veterinarianId, Date: date },
    });
    return NextResponse.json(slots);
  } catch (e) {
    const status = e instanceof BackendError ? e.status : 500;
    return NextResponse.json(
      { error: e instanceof Error ? e.message : "Erro ao buscar horários." },
      { status },
    );
  }
}
