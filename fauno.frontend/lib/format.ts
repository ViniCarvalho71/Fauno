// Formatadores de data/hora pt-BR e helpers de conversao pro backend.

export function toIsoDate(d: Date): string {
  // yyyy-MM-dd em horario local (sem deslocar pra UTC).
  const y = d.getFullYear();
  const m = String(d.getMonth() + 1).padStart(2, "0");
  const day = String(d.getDate()).padStart(2, "0");
  return `${y}-${m}-${day}`;
}

// "HH:mm:ss" -> "HH:mm"
export function trimSeconds(time: string): string {
  return time.slice(0, 5);
}

// Combina "yyyy-MM-dd" + "HH:mm:ss" num DateTime local sem timezone,
// no formato que o backend .NET espera (DateTime "Unspecified").
export function combineDateTime(date: string, time: string): string {
  return `${date}T${time.length === 5 ? time + ":00" : time}`;
}

export function formatDateTime(iso: string): string {
  const d = new Date(iso);
  if (Number.isNaN(d.getTime())) return iso;
  return d.toLocaleString("pt-BR", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  });
}

export function formatTimeRange(startIso: string, endIso: string): string {
  const s = new Date(startIso);
  const e = new Date(endIso);
  const fmt = (d: Date) =>
    d.toLocaleTimeString("pt-BR", { hour: "2-digit", minute: "2-digit" });
  return `${fmt(s)} – ${fmt(e)}`;
}

export function formatLongDate(iso: string): string {
  const d = new Date(iso + (iso.length === 10 ? "T00:00:00" : ""));
  return d.toLocaleDateString("pt-BR", {
    weekday: "long",
    day: "2-digit",
    month: "long",
  });
}
