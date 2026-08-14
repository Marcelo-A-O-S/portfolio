import { getServerSession } from "next-auth";
import { Sidebar, SidebarContent, SidebarGroup, SidebarGroupContent, SidebarGroupLabel, SidebarHeader, SidebarMenu, SidebarMenuButton, SidebarMenuItem, SidebarTrigger } from "./ui/sidebar";
import { Home, Package, Wrench, Globe, ChartBarStacked, FolderKanban, ShieldCheck, Link as LinkType, UsersRound, LayoutDashboard } from "lucide-react";
import Link from "next/link";
import { headers } from "next/headers";
import { authOptions } from "@/lib/auth";
const itemsManager = [
    {
        title: "Dashboard",
        url: "/dashboard",
        icon: LayoutDashboard 
    },
    {
        title: "Certificates",
        url: "/admin/certificates",
        icon: ShieldCheck
    },
    {
        title: "Projects",
        url: "/admin/projects",
        icon: FolderKanban
    },
    {
        title: "Tools",
        url: "/admin/tools",
        icon: Wrench
    },
    {
        title: "Links Types",
        url: "/admin/links-types",
        icon: LinkType
    },
    {
        title: "Categories",
        url: "/admin/categories",
        icon: ChartBarStacked
    },
    {
        title: "Languages",
        url: "/admin/languages",
        icon: Globe
    },
    {
        title: "Users",
        url: "/admin/users",
        icon: UsersRound 
    }
]
export async function AppSidebar() {
    const session = await getServerSession(authOptions);
    if (!session?.user) {
        return
    }
    if(session?.user.role !== "Administrador" && session?.user.role !== "Moderator"){
        console.log("User:", session.user)
        return;
    }
    return  (
        <>
            <SidebarTrigger className="fixed z-20 top-11" />
            <Sidebar collapsible="icon" className="container" variant="floating">
                <SidebarContent className="flex flex-col justify-center">
                    <SidebarGroup>
                        <SidebarGroupLabel>Application</SidebarGroupLabel>
                        <SidebarGroupContent>
                            <SidebarMenu>
                                {itemsManager.map((item) => (
                                    <SidebarMenuItem key={item.title}>
                                        <SidebarMenuButton asChild>
                                            <Link className="" href={item.url}>
                                                <item.icon />
                                                <span>{item.title}</span>
                                            </Link>
                                        </SidebarMenuButton>
                                    </SidebarMenuItem>
                                ))}
                            </SidebarMenu>
                        </SidebarGroupContent>
                    </SidebarGroup>
                </SidebarContent>
            </Sidebar>
        </>
    )
}