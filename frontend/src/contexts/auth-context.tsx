import { logout } from "@/services/client/auth-services";
import { useSession } from "next-auth/react";
import { createContext, ReactNode, useContext, useEffect, useRef, useState } from "react";
type AuthContextProps = {
    isLoading: boolean;
    isAuthenticated: boolean;
    isSigningOut: boolean;
    isLoginModalOpen: boolean;
    openLoginModal: () => void;
    closeLoginModal: () => void;
}
type AuthProviderProps = {
    children: ReactNode
}
const authErrors = [
    "RefreshAccessTokenError",
    "RefreshTokenMissing"
];
export const AuthContext = createContext<AuthContextProps | undefined>(undefined);
export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error("useAuth deve ser utilizado dentro de AuthProvider");
    }
    return context;
}
export const AuthProvider = ({ children }: AuthProviderProps) => {
    const { data: session, status } = useSession();
    const [isLoginModalOpen, setIsLoginModalOpen] = useState(false);
    const logoutStarted = useRef(false);
    const isSigningOut =
        status === "authenticated" &&
        !!session?.error;
    useEffect(() => {
        if (status !== "authenticated") {
            return;
        }
        if (!session?.error) {
            logoutStarted.current = false;
            return;
        }
        if (!authErrors.includes(session.error)) {
            return;
        }
        if (logoutStarted.current) {
            return;
        }
        logoutStarted.current = true;
        void logout().catch((error) => {
            console.error("Falha ao encerrar a sessão:", error);
        });
    }, [session?.error, status]);
    const openLoginModal = () => setIsLoginModalOpen(true);
    const closeLoginModal = () => setIsLoginModalOpen(false);
    const value: AuthContextProps = {
        isLoading: status === "loading" || isSigningOut,
        isAuthenticated:
            status === "authenticated" &&
            !session?.error &&
            !isSigningOut,
        isSigningOut,
        isLoginModalOpen,
        openLoginModal,
        closeLoginModal
    };
    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    )
}