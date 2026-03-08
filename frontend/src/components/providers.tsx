"use client"

import { SessionProvider } from "next-auth/react"
import { ThemeProvider } from "@/providers/theme-provider"
import { MusicProvider } from "@/contexts/MusicContext"

export default function Providers({ children }: { children: React.ReactNode }) {
  return (
    <MusicProvider>
      <SessionProvider>
        <ThemeProvider
          attribute="class"
          defaultTheme="system"
          enableSystem
          disableTransitionOnChange
        >
          {children}
        </ThemeProvider>
      </SessionProvider>
    </MusicProvider>
  )
}