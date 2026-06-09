import "server-only";
import { getToken } from "./session";

// Camada que fala com as 3 APIs .NET a partir do servidor (route handlers).
// Resolve CORS (browser nunca chama .NET direto) e injeta o Bearer da sessao.

type Api = "auth" | "agenda" | "register";

function baseUrl(api: Api): string {
  switch (api) {
    case "auth":
      return process.env.AUTH_API_URL ?? "http://localhost:8080";
    case "agenda":
      return process.env.AGENDA_API_URL ?? "http://localhost:8081";
    case "register":
      return process.env.REGISTER_API_URL ?? "http://localhost:8082";
  }
}

export class BackendError extends Error {
  status: number;
  constructor(status: number, message: string) {
    super(message);
    this.status = status;
  }
}

interface BackendOptions {
  method?: string;
  body?: unknown;
  token?: string | null; // sobrescreve o token da sessao (ex.: logo apos login)
  auth?: boolean; // default true: anexa Bearer
  query?: Record<string, string | undefined>;
}

export async function backend<T = unknown>(
  api: Api,
  path: string,
  opts: BackendOptions = {},
): Promise<T> {
  const { method = "GET", body, auth = true, query } = opts;

  let url = `${baseUrl(api)}/${path.replace(/^\//, "")}`;
  if (query) {
    const params = new URLSearchParams();
    for (const [k, v] of Object.entries(query)) {
      if (v !== undefined && v !== null && v !== "") params.set(k, v);
    }
    const qs = params.toString();
    if (qs) url += `?${qs}`;
  }

  const headers: Record<string, string> = {};
  if (body !== undefined) headers["Content-Type"] = "application/json";

  if (auth) {
    const token = opts.token !== undefined ? opts.token : await getToken();
    if (token) headers["Authorization"] = `Bearer ${token}`;
  }

  let res: Response;
  try {
    res = await fetch(url, {
      method,
      headers,
      body: body !== undefined ? JSON.stringify(body) : undefined,
      cache: "no-store",
    });
  } catch {
    // Falha de rede (API .NET fora do ar, DNS, etc.)
    throw new BackendError(503, "Não foi possível conectar ao servidor.");
  }

  const text = await res.text();

  if (!res.ok) {
    // As APIs devolvem a mensagem de erro como string crua no BadRequest.
    let message = text;
    try {
      const parsed = JSON.parse(text);
      message = parsed?.erro ?? parsed?.message ?? parsed?.mensagem ?? text;
    } catch {
      /* texto cru */
    }
    throw new BackendError(res.status, message || `Erro ${res.status}`);
  }

  if (!text) return undefined as T;
  try {
    return JSON.parse(text) as T;
  } catch {
    return text as unknown as T;
  }
}
