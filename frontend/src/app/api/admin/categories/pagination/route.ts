import { categoriesFilters, CategoriesFilters } from "@/domain/schemas/CategoriesFilters";
import { validateUserByRequest } from "@/services/server/auth-services";
import { getCategoriesByPaginationService } from "@/services/server/category-services";
import { NextRequest, NextResponse } from "next/server";
export async function GET(request: NextRequest) {
    const allowed = await validateUserByRequest(request, ["Administrador"]);
    if (!allowed)
        return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
    const searchParams = request.nextUrl.searchParams;
    const filters: CategoriesFilters = {
        page: Number(searchParams.get("page")) || 1,
        language: searchParams.get("language") || undefined,
        search: searchParams.get("search") || undefined
    }
    const result = await categoriesFilters.safeParseAsync(filters);
    if (!result.success) {
        return NextResponse.json({
            message: `Erro ao validar dados: ${result.error.message}`
        }, {
            status: 400
        })
    }
    const data = result.data;
    const response = await getCategoriesByPaginationService(data);
    if (response.status !== 200 && response.status !== 201) {
        return NextResponse.json({
            message: response.data.message
        }, {
            status: response.status
        })
    }
    return NextResponse.json(response.data);
}