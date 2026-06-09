import { NextResponse } from "next/server";
import { backend, BackendError } from "@/lib/backend";
import { getSession } from "@/lib/session";
import type { Pet } from "@/lib/types";

// Atualiza um pet.
export async function PUT(
  req: Request,
  { params }: { params: Promise<{ id: string }> },
) {
  const session = await getSession();
  if (!session || session.role !== "owner") {
    return NextResponse.json({ error: "Não autorizado." }, { status: 403 });
  }
  try {
    const { id } = await params;
    const { nome, especie, raca } = await req.json();
    if (!nome || !especie || !raca) {
      return NextResponse.json(
        { error: "Preencha nome, espécie e raça." },
        { status: 400 },
      );
    }
    const pet = await backend<Pet>("register", `api/cadastros/pets/${id}`, {
      method: "PUT",
      body: { nome, especie, raca },
    });
    return NextResponse.json(pet);
  } catch (e) {
    const status = e instanceof BackendError ? e.status : 500;
    return NextResponse.json(
      { error: e instanceof Error ? e.message : "Erro ao atualizar pet." },
      { status },
    );
  }
}
