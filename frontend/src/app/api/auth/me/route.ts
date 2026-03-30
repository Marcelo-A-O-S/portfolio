import { getToken } from "next-auth/jwt";
import { NextRequest, NextResponse } from "next/server";

const authSecret = process.env.NEXTAUTH_SECRET!;
export async function GET(request: NextRequest) {
    try {
        const token = await getToken({
            req: request,
            secret: authSecret
        })
        if (!token || !token.userId)
            return NextResponse.json({ error: "Unauthorized" }, { status: 401 })
        return NextResponse.next();
    } catch (err) {
        return NextResponse.json(
            ((err as Error).message),
            { status: 401 }
        );
    }
}