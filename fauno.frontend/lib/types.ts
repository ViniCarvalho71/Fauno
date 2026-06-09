// Tipos espelhando os DTOs das APIs .NET.
// Enums batem com os inteiros do backend (ordem das declaracoes C#).

export type Role = "owner" | "vet";

// AppointmentType (C# enum -> int)
export enum AppointmentType {
  Consultation = 0,
  Return = 1,
  Vaccination = 2,
  Exam = 3,
}

export const APPOINTMENT_TYPE_LABEL: Record<AppointmentType, string> = {
  [AppointmentType.Consultation]: "Consulta",
  [AppointmentType.Return]: "Retorno",
  [AppointmentType.Vaccination]: "Vacinação",
  [AppointmentType.Exam]: "Exame",
};

// AppointmentStatus vem como string no response do backend.
export type AppointmentStatus =
  | "Scheduled"
  | "Confirmed"
  | "Cancelled"
  | "Finished";

export const APPOINTMENT_STATUS_LABEL: Record<AppointmentStatus, string> = {
  Scheduled: "Agendada",
  Confirmed: "Confirmada",
  Cancelled: "Cancelada",
  Finished: "Finalizada",
};

// RecurrenceMode (C# enum -> int)
export enum RecurrenceMode {
  Weekly = 0,
  SpecificDates = 1,
}

// DayOfWeek do .NET: Domingo = 0 ... Sabado = 6
export const WEEKDAYS: { value: number; label: string; short: string }[] = [
  { value: 0, label: "Domingo", short: "Dom" },
  { value: 1, label: "Segunda", short: "Seg" },
  { value: 2, label: "Terça", short: "Ter" },
  { value: 3, label: "Quarta", short: "Qua" },
  { value: 4, label: "Quinta", short: "Qui" },
  { value: 5, label: "Sexta", short: "Sex" },
  { value: 6, label: "Sábado", short: "Sáb" },
];

export interface Pet {
  id: string;
  nome: string;
  especie: string;
  raca: string;
  donoId: string;
}

export interface OwnerProfile {
  id: string;
  userId: string;
  nome: string;
  email: string;
  cpf?: unknown;
}

export interface VetProfile {
  id: string;
  userId: string;
  nome: string;
  crmv: string;
  cpf?: unknown;
}

export interface AppointmentResponse {
  id: string;
  title?: string | null;
  description?: string | null;
  status: AppointmentStatus;
  appointmentType: string; // backend devolve nome do enum como string
  veterinarianId: string;
  veterinarianName: string;
  ownerId: string;
  ownerName: string;
  petId: string;
  petName: string;
  start: string; // ISO date-time
  end: string;
  createdAt: string;
}

export interface AvailableSlot {
  date: string; // yyyy-MM-dd
  start: string; // HH:mm:ss
  end: string; // HH:mm:ss
}

// ---- Payloads de criacao ----

export interface MakeAppointmentPayload {
  veterinarianId: string;
  petId: string;
  title?: string | null;
  description?: string | null;
  start: string; // yyyy-MM-ddTHH:mm:ss
  end: string;
  appointmentType: AppointmentType;
}

export interface PauseWindow {
  start: string; // HH:mm
  end: string;
}

export interface RecurrencePayload {
  mode: RecurrenceMode;
  daysOfWeek?: number[] | null;
  periodStart?: string | null; // yyyy-MM-dd
  periodEnd?: string | null;
  dates?: string[] | null;
}

export interface CreateAvailabilityRulePayload {
  slotStart: string; // HH:mm
  slotEnd: string;
  slotDurationMinutes: number;
  pause?: PauseWindow | null;
  recurrence: RecurrencePayload;
}

export interface CreateAvailabilityExceptionPayload {
  date: string; // yyyy-MM-dd
  reason?: string | null;
}

export interface SessionInfo {
  role: Role;
  profileId: string;
  userId: string;
  name: string;
}
