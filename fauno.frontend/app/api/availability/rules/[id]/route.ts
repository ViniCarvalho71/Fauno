import { NextResponse } from "next/server";
import { backend, BackendError } from "@/lib/backend";
import { getSession } from "@/lib/session";

// Remove uma regra de disponibilidade pelo id (vet).
export async function DELETE(
  _req: Request,
  { params }: { params: Promise<{ id: string }> },
) {
  const session = await getSession();
  if (!session || session.role !== "vet") {
    return NextResponse.json({ error: "Não autorizado." }, { status: 403 });
  }
  try {
    const { id } = await params;
    await backend("agenda", `AvailabilityRule/${id}`, { method: "DELETE" });
    return NextResponse.json({ ok: true });
  } catch (e) {
    const status = e instanceof BackendError ? e.status : 500;
    return NextResponse.json(
      { error: e instanceof Error ? e.message : "Erro ao remover regra." },
      { status },
    );
  }
}
