"use client"

import {
    Sidebar,
    SidebarContent,
    SidebarFooter,
    SidebarGroup,
    SidebarGroupAction,
    SidebarGroupContent,
    SidebarGroupLabel,
    SidebarHeader,
    SidebarMenu,
    SidebarMenuItem,
} from "@/components/ui/sidebar"
import { SidebarMenuButton } from "@/components/ui/sidebar"
import { UsersIcon, MoonIcon, SunIcon, CalendarBlankIcon, SignOutIcon } from "@phosphor-icons/react"
import { Switch } from "@/components/ui/switch";
import { useTheme } from "next-themes";
import { useEffect, useState } from "react";
import { Avatar, AvatarFallback, AvatarImage } from "./ui/avatar";
import { usePathname, useRouter } from "next/navigation";

type SidebarItem = {
    icon: React.ReactNode;
    label: string;
    href: string;
}

const sidebarItems : SidebarItem[] = [
    {
        icon: <UsersIcon />,
        label: "Clientes",
        href: "/clientes",
    },
    {
        icon: <CalendarBlankIcon />,
        label: "Agendamentos",
        href: "/agendamentos",
    },
]


export function AppSidebar() {
    const [mounted, setMounted] = useState(false);
    const [activeItem, setActiveItem] = useState<SidebarItem | null>(null);
    const { theme, setTheme } = useTheme();
    const pathname = usePathname();
    const router = useRouter();
    
    useEffect(() => {
        setMounted(true);
        const item = sidebarItems.find((item) => item.href === pathname);
        if (item) {
            setActiveItem(item);
        }
    }, [pathname]);

    if (!mounted) return null;

    const toggleTheme = () => {
        setTheme(theme === "dark" ? "light" : "dark");
    }

    const handleItemClick = (item: SidebarItem) => {
        setActiveItem(item);
        router.push(item.href);
    }

    return (
        <Sidebar>
            <SidebarHeader>
                <SidebarMenu>
                    <SidebarMenuItem className="flex justify-center items-center py-2 border-b border-sidebar-border">
                        <h1 className="text-2xl font-bold">
                            Fauno
                        </h1>
                    </SidebarMenuItem>
                </SidebarMenu>
            </SidebarHeader>
            <SidebarContent>
                {sidebarItems.map((item) => (
                    <SidebarGroup>
                        <SidebarGroupContent>
                            <SidebarMenuButton className="rounded-r-md" isActive={activeItem?.href === item.href} onClick={() => handleItemClick(item)} asChild>
                                <div className="flex items-center gap-2">
                                    {item.icon}
                                    <span className="text-base font-bold">{item.label}</span>
                                </div>
                            </SidebarMenuButton>
                        </SidebarGroupContent>
                    </SidebarGroup>
                ))}
            </SidebarContent>

            <SidebarFooter>
                <SidebarMenu>
                    <SidebarGroup>
                        <SidebarGroupContent>
                            <SidebarMenuItem>
                                <div className="flex items-center space-x-2 px-2 pb-5 border-b justify-between">
                                    <span className="text-base font-bold">Tema</span>
                                    <div className="flex items-center space-x-2 text-lg">
                                        <SunIcon />
                                        <Switch id="theme-mode" checked={theme === "dark"} onCheckedChange={toggleTheme} />
                                        <MoonIcon />
                                    </div>
                                </div>
                            </SidebarMenuItem>
                        </SidebarGroupContent>
                    </SidebarGroup>
                    
                    <SidebarGroup>
                        <SidebarGroupContent>
                            <SidebarMenuItem>
                                <SidebarMenuButton>
                                    <Avatar>
                                        <AvatarImage src="" />
                                        <AvatarFallback>US</AvatarFallback>
                                    </Avatar>
                                    <span className="text-base font-bold">Username</span>
                                </SidebarMenuButton>
                            </SidebarMenuItem>
                        </SidebarGroupContent>
                        <SidebarGroupAction>
                            <SignOutIcon />
                        </SidebarGroupAction>
                    </SidebarGroup>
                </SidebarMenu>
            </SidebarFooter>
        </Sidebar>
    );
}