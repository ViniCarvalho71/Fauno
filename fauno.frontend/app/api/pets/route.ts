import { NextResponse } from "next/server";
import { backend, BackendError } from "@/lib/backend";
import { getSession } from "@/lib/session";
import type { Pet } from "@/lib/types";

// Lista os pets do dono logado.
export async function GET() {
  const session = await getSession();
  if (!session || session.role !== "owner") {
    return NextResponse.json({ error: "Não autorizado." }, { status: 403 });
  }
  try {
    const pets = await backend<Pet[]>(
      "register",
      `api/cadastros/pets/dono/${session.profileId}`,
    );
    return NextResponse.json(pets);
  } catch (e) {
    const status = e instanceof BackendError ? e.status : 500;
    return NextResponse.json(
      { error: e instanceof Error ? e.message : "Erro ao listar pets." },
      { status },
    );
  }
}

// Cadastra um pet para o dono logado.
export async function POST(req: Request) {
  const session = await getSession();
  if (!session || session.role !== "owner") {
    return NextResponse.json({ error: "Não autorizado." }, { status: 403 });
  }
  try {
    const { nome, especie, raca } = await req.json();
    if (!nome || !especie || !raca) {
      return NextResponse.json(
        { error: "Preencha nome, espécie e raça." },
        { status: 400 },
      );
    }
    const pet = await backend<Pet>("register", "api/cadastros/pets", {
      method: "POST",
      body: { nome, especie, raca, donoId: session.profileId },
    });
    return NextResponse.json(pet, { status: 201 });
  } catch (e) {
    const status = e instanceof BackendError ? e.status : 500;
    return NextResponse.json(
      { error: e instanceof Error ? e.message : "Erro ao cadastrar pet." },
      { status },
    );
  }
}
