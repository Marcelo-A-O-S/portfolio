import { getServerSession } from "next-auth";
import { Sidebar, SidebarContent, SidebarGroup, SidebarGroupContent, SidebarGroupLabel, SidebarHeader, SidebarMenu, SidebarMenuButton, SidebarMenuItem, SidebarTrigger } from "./ui/sidebar";
import { Home, Package, Wrench, Globe, ChartBarStacked, FolderKanban, ShieldCheck, Link as LinkType } from "lucide-react";
import Link from "next/link";
import { headers } from "next/headers";
const itemsManager = [
    {
        title: "Home",
        url: "/dashboard",
        icon: Home
    },
    {
        title: "Certificates",
        url: "/dashboard",
        icon: ShieldCheck
    },
    {
        title: "Projects",
        url: "/dashboard",
        icon: FolderKanban
    },
    {
        title: "Tools",
        url: "/dashboard",
        icon: Wrench
    },
    {
        title: "Type Links",
        url: "/dashboard",
        icon: LinkType
    },
    {
        title: "Categories",
        url: "/dashboard",
        icon: ChartBarStacked
    },
    {
        title: "Languages",
        url: "/admin/languages",
        icon: Globe
    }
]
export async function AppSidebar() {
    const session = await getServerSession();
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