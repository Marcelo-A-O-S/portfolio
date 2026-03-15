import { validateUserByRequest } from "@/services/server/auth-services";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request:NextRequest){
    const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
}