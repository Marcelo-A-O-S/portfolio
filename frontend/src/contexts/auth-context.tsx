import { logout } from "@/services/client/auth-services";
import { useSession } from "next-auth/react";
import { createContext, ReactNode, useEffect, useState } from "react";
type AuthContextProps = {
    loading: boolean
}
export const AuthContext = createContext({});
type AuthProviderProps = {
    children: ReactNode
}
export const AuthProvider = ({ children }: AuthProviderProps) => {
    const { data: session } = useSession();
    const [loading, setLoading] = useState(true);
    const checkAuth = async () => {
        try {
            const response = await fetch("/api/auth/me", {
                method: "GET",
                headers: {
                    "Content-Type": "application/json"
                }
            });
            if (!response.ok) {
                await logout();
            }
        } catch (error) {
            console.log("Verificação de autenticação retornou uma falha:", error);
        } finally {
            setLoading(false);
        }
    }
    return (
        <AuthContext.Provider value={{}}>
            {children}
        </AuthContext.Provider>
    )
}