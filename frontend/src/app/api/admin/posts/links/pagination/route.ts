import { linkFilters } from "@/domain/schemas/LinkFilters";
import { handleApiError } from "@/lib/api-error";
import { validateUserByRequest } from "@/services/server/auth-services";
import { getLinksByPagination } from "@/services/server/link-services";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
        const searchParams = request.nextUrl.searchParams;
        const page = Number(searchParams.get("page")) || 1;
        const postId = searchParams.get("postId") || undefined;
        const search = searchParams.get("search") || undefined;
        if (postId == undefined)
            return NextResponse.json({ message: "Identificador da publicação é obrigatório" }, { status: 400 });
        const data = {
            page,
            postId,
            search
        }
        const result = await linkFilters.safeParseAsync(data);
        if (!result.success) {
            return NextResponse.json({
                message: `Erro ao validar dados: ${result.error.message}`
            }, {
                status: 400
            })
        }
        const filters = result.data;
        const response = await getLinksByPagination(filters);
        if (response.status !== 200 && response.status !== 201) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            })
        }
        return NextResponse.json(response.data);
    } catch (error) {
        return handleApiError(error);
    }
}