import { logout } from "@/services/server/auth-services";
import { getToken } from "next-auth/jwt";
import { cookies } from "next/headers";
import { NextRequest, NextResponse } from "next/server";
const authSecret = process.env.NEXTAUTH_SECRET!;
export async function POST(request: NextRequest) {
    const token = await getToken({
        req: request,
        secret: authSecret
    })
    if (!token || !token.userId)
        return NextResponse.json({ error: "Unauthorized" }, { status: 401 })
    const cookieStore = await cookies();
    const deviceId = cookieStore.get("DeviceId")?.value;
    if (!deviceId) {
        cookieStore.delete("DeviceId");
        cookieStore.delete("RefreshToken");
        return NextResponse.json({ success: true });
    }
    try{
        await logout(token.userId, deviceId);
    }catch {}
    return NextResponse.json({ success: true });
}