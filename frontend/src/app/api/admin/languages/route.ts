import { languageSchema } from "@/domain/schemas/LanguageSchema";
import { ApiErrorResponse } from "@/domain/types/ApiErrorResponse";
import { validateUserByRequest } from "@/services/server/auth-services";
import { addLanguageService, getLanguages } from "@/services/server/language-services";
import axios from "axios";
import { NextRequest, NextResponse } from "next/server";
export async function POST(request: NextRequest) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 })
        const data = await request.json();
        const result = await languageSchema.safeParseAsync(data);
        if (!result.success) {
            return NextResponse.json({
                message: `Erro ao validar dados: ${result.error.message}`
            }, {
                status: 400
            });
        }
        const language = result.data;
        const response = await addLanguageService(language);
        if (response.status !== 200 && response.status !== 201) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            });
        }
        return NextResponse.json(response.data);
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
export async function GET(request: NextRequest) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
        const response = await getLanguages();
        if (response.status !== 200 && response.status !== 201) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            })
        }
        return NextResponse.json(response.data);
    }catch(error){
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