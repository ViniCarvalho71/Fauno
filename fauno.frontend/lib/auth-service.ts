import "server-only";
import { backend, BackendError } from "./backend";
import { getUserIdFromToken } from "./jwt";
import { setSession } from "./session";
import type { Role } from "./types";

interface LoginResponse {
  token: string;
  expiresAt: string;
}

interface ResolvedRole {
  role: Role;
  profileId: string;
  name: string;
}

// Descobre se o userId logado e Dono ou Veterinario sondando o Register.
// O JWT nao carrega papel; sondamos donos/usuario/{id}/id e, se nao achar,
// veterinarios/usuario/{id}/id.
async function resolveRole(
  userId: string,
  token: string,
): Promise<ResolvedRole> {
  // tenta Dono
  try {
    const res = await backend<{ ownerId: string }>(
      "register",
      `api/cadastros/donos/usuario/${userId}/id`,
      { token },
    );
    const ownerId = res.ownerId;
    const dono = await backend<{ nome: string }>(
      "register",
      `api/cadastros/donos/${ownerId}`,
      { token },
    );
    return { role: "owner", profileId: ownerId, name: dono?.nome ?? "" };
  } catch (e) {
    if (!(e instanceof BackendError) || e.status !== 404) {
      // erro diferente de "nao encontrado": tenta vet mesmo assim abaixo
    }
  }

  // tenta Veterinario
  const res = await backend<{ veterinarianId: string }>(
    "register",
    `api/cadastros/veterinarios/usuario/${userId}/id`,
    { token },
  );
  const vetId = res.veterinarianId;
  const vet = await backend<{ nome: string }>(
    "register",
    `api/cadastros/veterinarios/${vetId}`,
    { token },
  );
  return { role: "vet", profileId: vetId, name: vet?.nome ?? "" };
}

// Faz login no Auth, resolve papel e grava a sessao. Devolve o papel.
export async function loginAndCreateSession(
  email: string,
  password: string,
): Promise<Role> {
  const login = await backend<LoginResponse>("auth", "User/Login", {
    method: "POST",
    body: { email, password },
    auth: false,
  });

  const token = login.token;
  const userId = getUserIdFromToken(token);
  if (!userId) {
    throw new BackendError(500, "Token inválido recebido do Auth.");
  }

  const resolved = await resolveRole(userId, token);

  await setSession({
    token,
    role: resolved.role,
    profileId: resolved.profileId,
    userId,
    name: resolved.name,
  });

  return resolved.role;
}
