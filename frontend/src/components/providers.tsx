"use client"

import { SessionProvider } from "next-auth/react"
import { ThemeProvider } from "@/providers/theme-provider"
import { MusicProvider } from "@/contexts/music-context"
import { QueryClientProvider, QueryClient } from "@tanstack/react-query"
import { AuthProvider } from "@/contexts/auth-context"
const queryClient = new QueryClient();
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
          <AuthProvider>
            <QueryClientProvider client={queryClient}>
              {children}
            </QueryClientProvider>
          </AuthProvider>
        </ThemeProvider>
      </SessionProvider>
    </MusicProvider>
  )
}