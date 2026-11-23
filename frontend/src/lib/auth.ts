import "server-only";
import type { AuthOptions } from "next-auth";
import { LoginRequest } from "@/domain/dtos/LoginRequest";
import { buildDeviceName } from "@/lib/build-device-name";
import { accessLogin, loginOAuth, logout, refreshAccessToken, refreshAsync } from "@/services/server/auth-services";
import Github from "next-auth/providers/github";
import Google from "next-auth/providers/google";
import LinkedIn from "next-auth/providers/linkedin";
import { cookies } from "next/headers";
const githubId = process.env.GITHUB_ID!;
const githubSecret = process.env.GITHUB_SECRET!;
const githubIssuer = process.env.GITHUB_ISSUER!;
const googleId = process.env.GOOGLE_ID!;
const googleSecret = process.env.GOOGLE_SECRET!;
const linkedlnId = process.env.LINKEDLN_ID!;
const linkedlnSecret = process.env.LINKEDLN_SECRET!;
const linkedlnIssuer = process.env.LINKEDLN_ISSUER!;
const linkedlnbWellknown = process.env.LINKEDLN_WELLKNOWN!;
export const authOptions: AuthOptions = {
    secret: process.env.NEXTAUTH_SECRET!,
    providers: [
        Github({
            clientId: githubId,
            clientSecret: githubSecret,
            checks: ["state"],
            issuer: githubIssuer,
            profile(profile) {
                //bio
                return {
                    id: profile.id.toString(),
                    name: profile.name,
                    username: profile.login,
                    email: profile.email,
                    image: profile.avatar_url,
                    bio: profile.bio
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
            issuer: linkedlnIssuer,
            wellKnown: linkedlnbWellknown,
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
                token = await accessLogin(token, account, user);
            }
            if(!token.expireIn){
                return token;
            }
            const refreshBuffer = 60 * 1000;
            if (Date.now() < token.expireIn - refreshBuffer) {
                return token;
            }
            return await refreshAccessToken(token);
        },
        async session({ session, token, user }) {
            session.user.username = token.username;
            session.user.id = token.userId;
            session.user.role = token.role;
            session.error = token.error;
            return session;
        }
    }
}
