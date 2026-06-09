"use client";

import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarGroup,
  SidebarGroupContent,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from "@/components/ui/sidebar";
import {
  PawPrintIcon,
  MoonIcon,
  SunIcon,
  CalendarBlankIcon,
  CalendarCheckIcon,
  ClockIcon,
  SignOutIcon,
  type Icon,
} from "@phosphor-icons/react";
import { Switch } from "@/components/ui/switch";
import { useTheme } from "next-themes";
import { useEffect, useState } from "react";
import { Avatar, AvatarFallback } from "./ui/avatar";
import { usePathname, useRouter } from "next/navigation";
import { toast } from "sonner";
import { api } from "@/lib/api-client";
import type { Role } from "@/lib/types";

type NavItem = { icon: Icon; label: string; href: string };

const OWNER_NAV: NavItem[] = [
  { icon: CalendarBlankIcon, label: "Agendar", href: "/agendar" },
  { icon: CalendarCheckIcon, label: "Minhas Consultas", href: "/consultas" },
  { icon: PawPrintIcon, label: "Meus Pets", href: "/pets" },
];

const VET_NAV: NavItem[] = [
  { icon: CalendarCheckIcon, label: "Minha Agenda", href: "/agenda" },
  { icon: ClockIcon, label: "Disponibilidade", href: "/disponibilidade" },
];

function initials(name: string): string {
  const parts = name.trim().split(/\s+/).filter(Boolean);
  if (parts.length === 0) return "US";
  return (parts[0][0] + (parts[1]?.[0] ?? "")).toUpperCase();
}

export function AppSidebar({ role, name }: { role: Role; name: string }) {
  const [mounted, setMounted] = useState(false);
  const { theme, setTheme } = useTheme();
  const pathname = usePathname();
  const router = useRouter();

  useEffect(() => setMounted(true), []);

  const nav = role === "vet" ? VET_NAV : OWNER_NAV;
  const roleLabel = role === "vet" ? "Veterinário" : "Dono";

  async function logout() {
    try {
      await api.post("/auth/logout");
      router.replace("/login");
      router.refresh();
    } catch {
      toast.error("Falha ao sair.");
    }
  }

  return (
    <Sidebar>
      <SidebarHeader>
        <SidebarMenu>
          <SidebarMenuItem className="flex items-center gap-2 px-2 py-3">
            <PawPrintIcon weight="fill" className="size-6" />
            <span className="text-xl font-bold tracking-tight">Fauno</span>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarHeader>

      <SidebarContent>
        <SidebarGroup>
          <SidebarGroupContent>
            <SidebarMenu>
              {nav.map((item) => {
                const active =
                  pathname === item.href ||
                  pathname.startsWith(item.href + "/");
                return (
                  <SidebarMenuItem key={item.href}>
                    <SidebarMenuButton
                      isActive={active}
                      onClick={() => router.push(item.href)}
                    >
                      <item.icon className="size-5" />
                      <span className="font-medium">{item.label}</span>
                    </SidebarMenuButton>
                  </SidebarMenuItem>
                );
              })}
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>
      </SidebarContent>

      <SidebarFooter>
        <SidebarMenu>
          {mounted && (
            <SidebarMenuItem>
              <div className="flex items-center justify-between px-2 py-1.5">
                <span className="text-sm font-medium">Tema</span>
                <div className="flex items-center gap-2">
                  <SunIcon className="size-4" />
                  <Switch
                    checked={theme === "dark"}
                    onCheckedChange={(c) => setTheme(c ? "dark" : "light")}
                    aria-label="Alternar tema"
                  />
                  <MoonIcon className="size-4" />
                </div>
              </div>
            </SidebarMenuItem>
          )}

          <SidebarMenuItem>
            <div className="flex items-center gap-2 border-t px-2 pt-3">
              <Avatar className="size-8">
                <AvatarFallback>{initials(name)}</AvatarFallback>
              </Avatar>
              <div className="flex min-w-0 flex-1 flex-col">
                <span className="truncate text-sm font-semibold">
                  {name || "Usuário"}
                </span>
                <span className="text-muted-foreground text-xs">
                  {roleLabel}
                </span>
              </div>
              <button
                onClick={logout}
                aria-label="Sair"
                className="hover:bg-sidebar-accent rounded-md p-1.5"
              >
                <SignOutIcon className="size-5" />
              </button>
            </div>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarFooter>
    </Sidebar>
  );
}
