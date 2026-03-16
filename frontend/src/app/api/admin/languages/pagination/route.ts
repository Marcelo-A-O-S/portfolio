import { languageFilters } from "@/domain/schemas/LanguageFilters";
import { ApiErrorResponse } from "@/domain/types/ApiErrorResponse";
import { validateUserByRequest } from "@/services/server/auth-services";
import { getLanguagesByPagination } from "@/services/server/language-services";
import axios from "axios";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 })
        const searchParams = request.nextUrl.searchParams;
        const page = Number(searchParams.get("page")) || 1;
        const search = searchParams.get("search") || undefined;
        const code = searchParams.get("code") || undefined;
        const data = {
            page,
            search,
            code
        }
        const result = await languageFilters.safeParseAsync(data);
        if (!result.success) {
            return NextResponse.json({
                message: `Erro ao validar dados: ${result.error.message}`
            }, {
                status: 400
            });
        }
        const language = result.data;
        const response = await getLanguagesByPagination(language);
        if (response.status !== 200 && response.status !== 201) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            });
        }
        return NextResponse.json(response.data);
    } catch (error: unknown) {
        if (axios.isAxiosError<ApiErrorResponse>(error)) {
            console.log("Erro backend:", error.response?.data);

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