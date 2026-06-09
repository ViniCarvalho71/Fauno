import { NextResponse } from "next/server";
import { backend, BackendError } from "@/lib/backend";
import { getSession } from "@/lib/session";
import type { VetProfile } from "@/lib/types";

// Lista todos os veterinarios. Usado pelo dono pra escolher o vet ao agendar.
export async function GET() {
  const session = await getSession();
  if (!session) {
    return NextResponse.json({ error: "Não autenticado." }, { status: 401 });
  }
  try {
    const vets = await backend<VetProfile[]>(
      "register",
      "api/cadastros/veterinarios",
    );
    return NextResponse.json(vets);
  } catch (e) {
    const status = e instanceof BackendError ? e.status : 500;
    return NextResponse.json(
      { error: e instanceof Error ? e.message : "Erro ao listar veterinários." },
      { status },
    );
  }
}
