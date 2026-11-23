import { LoginRequest } from "@/domain/dtos/LoginRequest";
import { buildDeviceName } from "@/lib/build-device-name";
import { Account, User } from "next-auth";
import { AdapterUser } from "next-auth/adapters";
import { decode, getToken, JWT } from "next-auth/jwt";
import { cookies } from "next/headers";
import { NextRequest } from "next/server";
type AuthTokenError =
    | "RefreshAccessTokenError"
    | "RefreshTokenMissing";
const host = process.env.BACKEND_SERVER!;
const hostNext = process.env.NEXT_BACKEND_API!;
const authSecret = process.env.NEXTAUTH_SECRET!;
export const loginOAuth = async (loginRequest: LoginRequest) => {
    const response = await fetch(`${host}/api/auth/login`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(loginRequest),
    });
    if (!response.ok) {
        const error = await response.json();
        console.log("Erro de console:", error);
        throw new Error(`Error: ${error}`);
    }
    const data = await response.json();
    return data;
}
export const refreshAsync = async (refreshTokenId: string, userId: string, providerId: string, refreshToken: string, deviceId: string, deviceName: string) => {
    const response = await fetch(`${host}/api/auth/refreshToken`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            refreshTokenId,
            userId,
            providerId,
            deviceId,
            deviceName,
            refreshToken,
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
    const cookieStore = await cookies();
    cookieStore.delete("DeviceId");
    cookieStore.delete("RefreshToken");
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
export const validateUserByRequest = async (req: NextRequest, roles: string[]) => {
    const token = await getToken({
        req,
        secret: authSecret
    })
    if (!token?.expireIn || !token.role) {
        return false;
    }
    if (!token?.role || !roles.includes(token.role)) {
        return false;
    }
    return true;
}
export const accessLogin = async (token: JWT, account: Account, user: User | AdapterUser): Promise<JWT> => {
    try {
        const cookieStore = await cookies();
        let deviceId = cookieStore.get("DeviceId")?.value;
        if (!deviceId) {
            deviceId = crypto.randomUUID();
        }
        const deviceName = await buildDeviceName();
        const provider = account.provider.toLowerCase();
        let providerToken: string | undefined = undefined;
        switch (provider) {
            case "github":
                providerToken = account.access_token
                break;
            case "linkedin":
                providerToken = account.access_token
                break;
            case "google":
                providerToken = account.id_token
                break;
        }
        const loginRequest: LoginRequest = {
            name: user.name,
            email: user.email,
            profileUrl: user.image,
            description: user.bio ?? "",
            provider: account.provider,
            providerId: account.providerAccountId,
            providerToken: providerToken,
            username: user.username,
            deviceId,
            deviceName
        }
        const data = await loginOAuth(loginRequest);
        try {
            cookieStore.set("DeviceId", deviceId, {
                httpOnly: true,
                sameSite: "lax",
                secure: true,
                path: "/",
                maxAge: 60 * 60 * 24 * 7
            });
            cookieStore.set("RefreshToken", data.refreshToken, {
                httpOnly: true,
                sameSite: "lax",
                secure: true,
                path: "/",
                maxAge: 60 * 60 * 24 * 7
            });
        } catch (error) {
            throw new Error("Erro ao gerenciar cookies.");
        }
        return {
            ...token,
            providerId: account.providerAccountId,
            provider: account.provider,
            username: user.username,
            userId: data.userId,
            accessToken: data.accessToken,
            refreshTokenId: data.refreshTokenId,
            role: data.role,
            expireIn: Date.now() + data.expireIn * 1000
        }
    } catch (error) {
        console.error("Erro ao acessar login:", error);
        return token;
    }
}
export const refreshAccessToken = async (token: JWT): Promise<JWT> => {
    try {
        const cookieStore = await cookies();
        const deviceId = cookieStore.get("DeviceId")?.value;
        const refreshToken = cookieStore.get("RefreshToken")?.value;
        if (!deviceId || !refreshToken || !token.userId ||
            !token.providerId || !token.refreshTokenId) {
            return {
                ...token,
                error: "RefreshTokenMissing" satisfies AuthTokenError
            };
        }
        const deviceName = await buildDeviceName();
        const response = await refreshAsync(
            token.refreshTokenId,
            token.userId,
            token.providerId,
            refreshToken,
            deviceId,
            deviceName
        );

        if (!response.ok) {
            await logout(token.userId, deviceId);
            return {
                ...token,
                error: "RefreshAccessTokenError" satisfies AuthTokenError
            };
        }
        const data = await response.json();
        if (
            !data.accessToken ||
            !data.refreshToken ||
            !data.refreshTokenId ||
            !data.expireIn ||
            !data.userId
        ) {
            await logout(token.userId, deviceId);
            throw new Error("Resposta inválida do AuthService.");
        }
        cookieStore.set("RefreshToken", data.refreshToken, {
            httpOnly: true,
            sameSite: "lax",
            secure: true,
            path: "/",
            maxAge: 60 * 60 * 24 * 7
        });
        return {
            ...token,
            userId: data.userId,
            accessToken: data.accessToken,
            refreshTokenId: data.refreshTokenId,
            expireIn: Date.now() + data.expireIn * 1000,
            role: data.role
        }
    } catch (error) {
        console.error("Falha ao renovar o token:", error);
        return {
            ...token,
            error: "RefreshAccessTokenError" satisfies AuthTokenError
        };
    }
}