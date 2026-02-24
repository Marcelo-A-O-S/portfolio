import { refreshAsync } from "@/services/server/auth-services";
import { getToken } from "next-auth/jwt";
import { cookies } from "next/headers";
import { NextRequest } from "next/server";
const authSecret = process.env.AUTH_SECRET!;
export async function POST(request: NextRequest){
    const token = await getToken({
        req: request,
        secret:authSecret
    })
    if(!token || !token.userId)
        return Response.json({ error: "Unauthorized"}, { status: 401});
    const cookieStore = await cookies();
    const deviceId = cookieStore.get("DeviceId")?.value;
    const refreshToken = cookieStore.get("RefreshToken")?.value;
    if(!deviceId || !refreshToken)
        return Response.json({error: "Unauthorized"}, { status: 401});
    
    const response = await refreshAsync(token.userId, refreshToken,deviceId);

    if(!response.ok){
        cookieStore.delete("RefreshToken");
        cookieStore.delete("DeviceId");
        return Response.json({ error: "Refresh failed"}, { status: 401});
    }
    const data = await response.json();
    return Response.json({
        accessToken: data.accessToken
    })
}