import { ApiErrorResponse } from "@/domain/types/ApiErrorResponse";
import { validateUserByRequest } from "@/services/server/auth-services";
import axios from "axios";
import { NextRequest, NextResponse } from "next/server";
export async function POST(request: NextRequest) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed) {
            return NextResponse.json({ error: "Unauthorized" }, { status: 401 });
        }
    } catch (error) {
        if (axios.isAxiosError<ApiErrorResponse>(error)) {
            console.log(error.response?.data);
            return NextResponse.json(
                {
                    message: error.response?.data?.message ?? "Erro no backend"
                },
                {
                    status: error.response?.status ?? 500
                }
            );
        }
    }
}