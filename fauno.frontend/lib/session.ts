import "server-only";
import { cookies } from "next/headers";
import type { Role, SessionInfo } from "./types";

// Cookies httpOnly da sessao. Token nunca chega ao browser.
const TOKEN = "fauno_token";
const ROLE = "fauno_role";
const PID = "fauno_pid"; // profileId (ownerId ou veterinarianId)
const UID = "fauno_uid"; // userId (do JWT)
const NAME = "fauno_name";

const baseCookie = {
  httpOnly: true,
  sameSite: "lax" as const,
  secure: process.env.NODE_ENV === "production",
  path: "/",
};

export async function setSession(data: {
  token: string;
  role: Role;
  profileId: string;
  userId: string;
  name: string;
}) {
  const store = await cookies();
  store.set(TOKEN, data.token, baseCookie);
  store.set(ROLE, data.role, baseCookie);
  store.set(PID, data.profileId, baseCookie);
  store.set(UID, data.userId, baseCookie);
  // nome serve so pra UI; pode ser nao-httpOnly, mas mantemos simples.
  store.set(NAME, encodeURIComponent(data.name), baseCookie);
}

export async function clearSession() {
  const store = await cookies();
  for (const name of [TOKEN, ROLE, PID, UID, NAME]) store.delete(name);
}

export async function getToken(): Promise<string | null> {
  const store = await cookies();
  return store.get(TOKEN)?.value ?? null;
}

export async function getSession(): Promise<SessionInfo | null> {
  const store = await cookies();
  const token = store.get(TOKEN)?.value;
  const role = store.get(ROLE)?.value as Role | undefined;
  const profileId = store.get(PID)?.value;
  const userId = store.get(UID)?.value;
  const name = store.get(NAME)?.value;
  if (!token || !role || !profileId || !userId) return null;
  return {
    role,
    profileId,
    userId,
    name: name ? decodeURIComponent(name) : "",
  };
}
