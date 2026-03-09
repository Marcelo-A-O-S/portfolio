"use client"

import { SessionProvider } from "next-auth/react"
import { ThemeProvider } from "@/providers/theme-provider"
import { MusicProvider } from "@/contexts/MusicContext"
import { QueryClientProvider, QueryClient} from "@tanstack/react-query"
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
          <QueryClientProvider client={queryClient}>
          {children}
          </QueryClientProvider>
        </ThemeProvider>
      </SessionProvider>
    </MusicProvider>
  )
}