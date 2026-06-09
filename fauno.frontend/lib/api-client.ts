// Cliente usado no browser. Chama SEMPRE o BFF (/api/*) do proprio Next,
// nunca as APIs .NET direto. O token vive em cookie httpOnly, entao aqui
// nao tem token nenhum — o servidor anexa.

export class ApiError extends Error {
  status: number;
  constructor(status: number, message: string) {
    super(message);
    this.status = status;
  }
}

async function request<T>(
  path: string,
  init?: RequestInit & { json?: unknown },
): Promise<T> {
  const { json, headers, ...rest } = init ?? {};
  const res = await fetch(`/api${path}`, {
    ...rest,
    headers: {
      ...(json !== undefined ? { "Content-Type": "application/json" } : {}),
      ...headers,
    },
    body: json !== undefined ? JSON.stringify(json) : rest.body,
  });

  const text = await res.text();
  let data: unknown = undefined;
  if (text) {
    try {
      data = JSON.parse(text);
    } catch {
      data = text;
    }
  }

  if (!res.ok) {
    const message =
      (data as { error?: string })?.error ??
      (typeof data === "string" ? data : null) ??
      `Erro ${res.status}`;
    throw new ApiError(res.status, message);
  }

  return data as T;
}

export const api = {
  get: <T>(path: string) => request<T>(path),
  post: <T>(path: string, json?: unknown) =>
    request<T>(path, { method: "POST", json }),
  put: <T>(path: string, json?: unknown) =>
    request<T>(path, { method: "PUT", json }),
  patch: <T>(path: string, json?: unknown) =>
    request<T>(path, { method: "PATCH", json }),
  del: <T>(path: string) => request<T>(path, { method: "DELETE" }),
};
