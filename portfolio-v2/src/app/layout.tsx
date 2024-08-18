import type { Metadata } from "next";
import { Inter } from "next/font/google";
import "./globals.css";
import Container from "./components/Container";
const inter = Inter({ subsets: ["latin"] });
import { Analytics } from '@vercel/analytics/react'
export const metadata: Metadata = {
  title: "Marcelo Augusto.Dev",
  description: "Olá, meu nome é Marcelo Augusto e sou estudante de programação com foco em desenvolvimento full stack nas horas vagas!",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="pt-br">
      <body className={" " + inter.className}>
        <Analytics/>
        <Container>
          {children}
        </Container>
        </body>
    </html>
  );
}
