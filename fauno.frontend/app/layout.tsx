import { Work_Sans } from "next/font/google"
import { SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar"
import { AppSidebar } from "@/components/app-sidebar"

import "./globals.css"
import { ThemeProvider } from "@/components/theme-provider"
import { cn } from "@/lib/utils";

const workSans = Work_Sans({ 
  subsets: ["latin"], // Especifique os subconjuntos necessários
  display: "swap",    // Melhora a experiência de carregamento
  weight: ["400"],
});

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode
}>) {
  return (
    <html
      lang="en"
      suppressHydrationWarning
      className={cn("antialiased", "font-mono", workSans.className)}
    >
      <body>
        <ThemeProvider>
          <SidebarProvider>
            <AppSidebar />
            <main className="flex gap-5 w-full">
              <SidebarTrigger />
              <div className="w-full">
                {children}
              </div>
            </main>
          </SidebarProvider>
        </ThemeProvider>
      </body>
    </html>
  )
}
