import { linkTypeFilters, LinkTypeFilters } from "@/domain/schemas/LinkTypeFilters";
import { ApiErrorResponse } from "@/domain/types/ApiErrorResponse";
import { validateUserByRequest } from "@/services/server/auth-services";
import { getLinkTypeByPagination } from "@/services/server/link-type-services";
import axios from "axios";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
        const searchParams = request.nextUrl.searchParams;
        const filters: LinkTypeFilters = {
            page: Number(searchParams.get("page")) || 1,
            search: searchParams.get("search") || undefined
        }
        const result = await linkTypeFilters.safeParseAsync(filters);
        if (!result.success) {
            return NextResponse.json({
                message: `Erro ao validar dados: ${result.error.message}`
            }, {
                status: 400
            })
        }
        const data = result.data;
        const response = await getLinkTypeByPagination(data);
        if (response.status !== 200 && response.status !== 201) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            })
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