import { ApiErrorResponse } from "@/domain/types/ApiErrorResponse";
import axios from "axios";
import { NextResponse } from "next/server";

export function handleApiError(error: unknown) {
    if (axios.isAxiosError<ApiErrorResponse>(error)) {
        console.log(error.response?.data);
        return NextResponse.json(
            { message: error.response?.data?.message ?? "Erro no backend" },
            { status: error.response?.status ?? 500 }
        );
    }
    console.error(error);
    return NextResponse.json(
        { message: "Erro interno do servidor" },
        { status: 500 }
    );
}