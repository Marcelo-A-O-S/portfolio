import { Metadata } from "next";
import FormContact from "../components/FormContact";
export const metadata: Metadata = {
    title: "Contact",
    description: "Contate-me para tratar de negócios",
  };
export default function ContactPage() {
    return (
        <>
            <div className="flex min-h-screen items-center justify-start bg-dark ">
                <div className="mx-auto w-full max-w-lg p-6">
                    <h1 className="text-4xl font-medium">Contate-me:</h1>
                    <p className="mt-3">Caso queira entrar em contato comigo para tratar de negócios:</p>
                    <FormContact/>
                </div>
            </div>
        </>)
}