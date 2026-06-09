import { NextResponse } from "next/server";
import { backend, BackendError } from "@/lib/backend";
import { getSession } from "@/lib/session";
import type { VetProfile } from "@/lib/types";

// Busca um veterinario por id. Usado pelo dono pra validar/mostrar o nome do
// vet ao agendar (o Register nao tem endpoint de listagem de vets).
export async function GET(
  _req: Request,
  { params }: { params: Promise<{ id: string }> },
) {
  const session = await getSession();
  if (!session) {
    return NextResponse.json({ error: "Não autenticado." }, { status: 401 });
  }
  try {
    const { id } = await params;
    const vet = await backend<VetProfile>(
      "register",
      `api/cadastros/veterinarios/${id}`,
    );
    return NextResponse.json(vet);
  } catch (e) {
    const status = e instanceof BackendError ? e.status : 500;
    return NextResponse.json(
      { error: e instanceof Error ? e.message : "Veterinário não encontrado." },
      { status },
    );
  }
}
