import { LoginRequest } from "@/domain/dtos/LoginRequest";
import { decode, getToken } from "next-auth/jwt";
import { cookies } from "next/headers";
import { NextRequest } from "next/server";
const host = process.env.BACKEND_SERVER!;
const authSecret = process.env.NEXTAUTH_SECRET!;
export const loginOAuth = async (loginRequest: LoginRequest) => {
    const response = await fetch(`${host}/api/auth/login`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(loginRequest),
    });
    if (!response.ok) throw new Error(`Èrror: ${response.status}`);
    const data = await response.json();
    return data;
}
export const refreshAsync = async (userId: string, refreshToken: string, deviceId: string) => {
    const response = await fetch(`${host}/api/auth/refreshToken`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            userId,
            deviceId,
            refreshToken
        })
    })
    return response;
}
export const logout = async (userId: string, deviceId: string) => {
    const response = await fetch(`${host}/api/auth/logout`, {
        method: "POST",
        headers: {
            "Content-type": "application/json"
        },
        body: JSON.stringify({
            userId,
            deviceId
        })
    })
    return response;
}
export const validateUser = async (roles: string[]) => {
    const cookieStore = await cookies();
    const tokenCookie = cookieStore.get("__Secure-next-auth.session-token")?.value ??
        cookieStore.get("next-auth.session-token")?.value;
    const token = await decode({
        secret: authSecret,
        token: tokenCookie
    });
    if (!token?.role || !roles.includes(token.role)) {
        return false;
    }
    return true;
}
export const validateUserByRequest = async (req: NextRequest, roles: string[]) =>{
    const token = await getToken({
        req,
        secret: authSecret
    })
    if (!token?.role || !roles.includes(token.role)) {
        return false;
    }
    return true;
}