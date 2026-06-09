import { NextResponse } from "next/server";
import { loginAndCreateSession } from "@/lib/auth-service";
import { BackendError } from "@/lib/backend";

export async function POST(req: Request) {
  try {
    const { email, password } = await req.json();
    if (!email || !password) {
      return NextResponse.json(
        { error: "Informe email e senha." },
        { status: 400 },
      );
    }
    const role = await loginAndCreateSession(email, password);
    return NextResponse.json({ role });
  } catch (e) {
    const status = e instanceof BackendError ? e.status : 500;
    const error =
      e instanceof Error ? e.message : "Falha ao entrar.";
    return NextResponse.json({ error }, { status });
  }
}
