import { NextResponse } from "next/server";
import { backend, BackendError } from "@/lib/backend";
import { loginAndCreateSession } from "@/lib/auth-service";

// Cadastro de Veterinario: cria o Vet (que cria o user no Auth) e ja loga.
export async function POST(req: Request) {
  try {
    const { nome, cpf, crmv, email, password } = await req.json();
    if (!nome || !cpf || !crmv || !email || !password) {
      return NextResponse.json(
        { error: "Preencha nome, CPF, CRMV, email e senha." },
        { status: 400 },
      );
    }

    await backend("register", "api/cadastros/veterinarios", {
      method: "POST",
      body: { nome, cpf, crmv, email, password },
      auth: false,
    });

    const role = await loginAndCreateSession(email, password);
    return NextResponse.json({ role });
  } catch (e) {
    const status = e instanceof BackendError ? e.status : 500;
    const error = e instanceof Error ? e.message : "Falha no cadastro.";
    return NextResponse.json({ error }, { status });
  }
}
