import { LoginRequest } from "@/domain/dtos/LoginRequest";
import { buildDeviceName } from "@/lib/build-device-name";
import { loginOAuth, logout, refreshAsync } from "@/services/server/auth-services";
import NextAuth from "next-auth";
import type { AuthOptions } from "next-auth";
import Github from "next-auth/providers/github";
import Google from "next-auth/providers/google";
import LinkedIn from "next-auth/providers/linkedin";
import { cookies } from "next/headers";
const hostNext = process.env.NEXT_BACKEND_API!;
const githubId = process.env.GITHUB_ID!;
const githubSecret = process.env.GITHUB_SECRET!;
const googleId = process.env.GOOGLE_ID!;
const googleSecret = process.env.GOOGLE_SECRET!;
const linkedlnId = process.env.LINKEDLN_ID!;
const linkedlnSecret = process.env.LINKEDLN_SECRET!;
export const authOptions: AuthOptions = {
    secret: process.env.NEXTAUTH_SECRET!,
    providers: [
        Github({
            clientId: githubId,
            clientSecret: githubSecret,
            profile(profile) {
                return {
                    id: profile.id.toString(),
                    name: profile.name,
                    username: profile.login,
                    email: profile.email,
                    image: profile.avatar_url
                }
            }
        }),
        Google({
            clientId: googleId,
            clientSecret: googleSecret,
            profile(profile) {
                return {
                    id: profile.sub,
                    name: profile.name,
                    username: `${profile.given_name} ${profile.family_name}`,
                    email: profile.email,
                    image: profile.picture
                }
            }
        }),
        LinkedIn({
            clientId: linkedlnId,
            clientSecret: linkedlnSecret,
            issuer: "https://www.linkedin.com/oauth",
            wellKnown: "https://www.linkedin.com/oauth/.well-known/openid-configuration",
            authorization: {
                params: {
                    scope: "openid profile email",
                },
            },
            profile(profile) {
                return {
                    id: profile.sub,
                    name: profile.name,
                    username: `${profile.given_name} ${profile.family_name}`,
                    email: profile.email,
                    image: profile.picture
                }
            }
        })
    ],
    callbacks: {
        async jwt({ token, account, user }) {
            if (account && user) {
                const cookieStore = await cookies();
                let deviceId = cookieStore.get("DeviceId")?.value;
                if (!deviceId) {
                    deviceId = crypto.randomUUID();
                }
                const deviceName = await buildDeviceName();
                const loginRequest: LoginRequest = {
                    name: user.name,
                    email: user.email,
                    profileUrl: user.image,
                    provider: account.provider,
                    providerId: account.providerAccountId,
                    username: user.username,
                    deviceId,
                    deviceName
                }
                const data = await loginOAuth(loginRequest);
                const expireDate = Date.now() + data.expireIn * 1000;
                token.provider = account.provider;
                token.username = user.username;
                token.userId = data.userId;
                token.accessToken = data.accessToken;
                token.refreshTokenId = data.refreshTokenId;
                token.expireIn = expireDate;
                token.role = data.role;
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
            }
            if (token.expireIn && token.userId && token.refreshTokenId) {
                const buffer = 35 * 1000;
                if (token.expireIn - buffer <= Date.now()) {
                    const cookieStore = await cookies();
                    const deviceId = cookieStore.get("DeviceId")?.value;
                    const refreshToken = cookieStore.get("RefreshToken")?.value;
                    if (deviceId && refreshToken) {
                        const deviceName = await buildDeviceName();
                        const response = await refreshAsync(token.refreshTokenId, token.userId, refreshToken, deviceId, deviceName)
                        if (response.status == 200 || response.status == 201) {
                            const data = await response.json();
                            const expireDate = Date.now() + data.expireIn * 1000;
                            token.userId = data.userId;
                            token.accessToken = data.accessToken;
                            token.refreshTokenId = data.refreshTokenId;
                            token.expireIn = expireDate;
                            token.role = data.role;
                            cookieStore.set("RefreshToken", data.refreshToken, {
                                httpOnly: true,
                                sameSite: "lax",
                                secure: true,
                                path: "/",
                                maxAge: 60 * 60 * 24 * 7
                            });
                            return token;
                        }
                        await logout(token.userId, deviceId);
                    }
                }
            }
            return token;
        },
        async session({ session, token, user }) {
            session.user.username = token.username;
            return session;
        }
    }

}
const handler = NextAuth(authOptions);
export { handler as GET, handler as POST };