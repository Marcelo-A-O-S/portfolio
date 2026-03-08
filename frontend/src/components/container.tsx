import { ReactNode } from "react"
import { ThemeProvider } from "@/providers/theme-provider"
import { SessionProvider } from "next-auth/react"
import { Toaster } from "./ui/sonner"
import Header from "./header"
import { MusicProvider } from "@/contexts/MusicContext"
import Providers from "./providers"
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
            </Providers>
        </>)
}