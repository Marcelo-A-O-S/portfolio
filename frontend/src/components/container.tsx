import { ReactNode } from "react"
import { Toaster } from "./ui/sonner"
import Header from "./header"
import Providers from "./providers"
import Footer from "./footer"
import { SidebarProvider, SidebarTrigger } from "./ui/sidebar"
import { AppSidebar } from "./app-sidebar"
type ContainerProps = {
    children: ReactNode
}

export default function Container({ children }: ContainerProps) {
    return (
        <>
            <Providers>
                <Header />
                <Toaster />
                {children}
                <Footer />
            </Providers>
        </>)
}