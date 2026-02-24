"use client"
import { ReactNode } from "react"
import { ThemeProvider } from "@/providers/theme-provider"
import Navbar from "./navbar"
import { SessionProvider } from "next-auth/react"

type ContainerProps = {
    children: ReactNode
}

export default function Container({ children }: ContainerProps) {

    return (
        <>
        <SessionProvider>
            <ThemeProvider
                attribute="class"
                defaultTheme="system"
                enableSystem
                disableTransitionOnChange>
                    <Navbar />
                    {children}
            </ThemeProvider>
        </SessionProvider>
        </>)
}