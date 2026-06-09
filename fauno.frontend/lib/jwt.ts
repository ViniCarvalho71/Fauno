// Decodifica o payload de um JWT (sem validar assinatura — so leitura de claim).
// O backend Auth emite o userId no claim NameIdentifier. Dependendo do mapeamento
// do JwtSecurityTokenHandler o claim pode aparecer como "nameid", a URI longa do
// ClaimTypes.NameIdentifier, ou "sub". Procuramos em todos.

const NAMEID_URI =
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

interface JwtPayload {
  [key: string]: unknown;
}

function base64UrlDecode(segment: string): string {
  const base64 = segment.replace(/-/g, "+").replace(/_/g, "/");
  const padded = base64.padEnd(
    base64.length + ((4 - (base64.length % 4)) % 4),
    "=",
  );
  // Em route handlers (Node) Buffer existe; atob como fallback.
  if (typeof Buffer !== "undefined") {
    return Buffer.from(padded, "base64").toString("utf-8");
  }
  return atob(padded);
}

export function decodeJwt(token: string): JwtPayload | null {
  const parts = token.split(".");
  if (parts.length < 2) return null;
  try {
    return JSON.parse(base64UrlDecode(parts[1])) as JwtPayload;
  } catch {
    return null;
  }
}

export function getUserIdFromToken(token: string): string | null {
  const payload = decodeJwt(token);
  if (!payload) return null;
  const candidate =
    payload["nameid"] ?? payload[NAMEID_URI] ?? payload["sub"];
  return typeof candidate === "string" ? candidate : null;
}
