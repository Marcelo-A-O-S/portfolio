import { NextRequest } from "next/server";
export async function PATCH(request: NextRequest, { params }: { params: Promise<{ Id: string }> }) {
    const data = await request.json();
    const { Id } = await params;
}