"use client";
import LoginForm from "./login-form";
import { useAuth } from "@/contexts/auth-context";
export function LoginModal(){
    const { isLoginModalOpen } = useAuth();
    if (!isLoginModalOpen) return null;
    return <LoginForm defaultOpen={true}/>
}