import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

// Guarda de rotas baseada na sessao (cookies httpOnly, lidos no edge).
// Next 16 usa a convencao "proxy" no lugar do antigo "middleware".
// Rotas publicas: /login e /cadastro. /api/* cuida da propria auth.

const OWNER_HOME = "/agendar";
const VET_HOME = "/agenda";
const PUBLIC = ["/login", "/cadastro"];

function homeFor(role: string | undefined): string {
  return role === "vet" ? VET_HOME : OWNER_HOME;
}

export function proxy(req: NextRequest) {
  const { pathname } = req.nextUrl;
  const token = req.cookies.get("fauno_token")?.value;
  const role = req.cookies.get("fauno_role")?.value;
  const isPublic = PUBLIC.some((p) => pathname.startsWith(p));

  // Sem sessao: so pode acessar rotas publicas.
  if (!token) {
    if (isPublic) return NextResponse.next();
    const url = req.nextUrl.clone();
    url.pathname = "/login";
    return NextResponse.redirect(url);
  }

  // Com sessao: redireciona da raiz e das telas publicas pra home do papel.
  if (isPublic || pathname === "/") {
    const url = req.nextUrl.clone();
    url.pathname = homeFor(role);
    return NextResponse.redirect(url);
  }

  return NextResponse.next();
}

export const config = {
  // Aplica a tudo, menos assets estaticos e /api (que valida sessao sozinho).
  matcher: ["/((?!api|_next/static|_next/image|favicon.ico).*)"],
};
