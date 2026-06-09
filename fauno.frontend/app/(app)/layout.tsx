import { redirect } from "next/navigation";
import { SidebarProvider, SidebarInset } from "@/components/ui/sidebar";
import { AppSidebar } from "@/components/app-sidebar";
import { getSession } from "@/lib/session";

export default async function AppLayout({
  children,
}: Readonly<{ children: React.ReactNode }>) {
  const session = await getSession();
  if (!session) redirect("/login");

  return (
    <SidebarProvider>
      <AppSidebar role={session.role} name={session.name} />
      <SidebarInset>{children}</SidebarInset>
    </SidebarProvider>
  );
}
