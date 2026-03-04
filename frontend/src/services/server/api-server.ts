import axios from "axios";
import { decode } from "next-auth/jwt";
import { cookies } from "next/headers";
const host = process.env.BACKEND_SERVER!
const authSecret = process.env.NEXTAUTH_SECRET!;
export const apiServer = async () => {
    const cookieStore = await cookies();
    const tokenCookie = cookieStore.get("__Secure-next-auth.session-token")?.value ??
        cookieStore.get("next-auth.session-token")?.value;
    const token = await decode({
        secret: authSecret,
        token: tokenCookie
    });
    const instance = axios.create({
        baseURL: host,
        headers: {
            "Content-Type": "application/json",
            "Authorization": token ? `Bearer ${token?.accessToken}` : "",
        }
    });
    return instance;
}