import { toolFilters } from "@/domain/schemas/ToolFilters";
import { validateUserByRequest } from "@/services/server/auth-services";
import { getToolsByPagination } from "@/services/server/tool-services";
import { NextRequest, NextResponse } from "next/server";
export async function GET(request: NextRequest) {
    const allowed = await validateUserByRequest(request, ["Administrador"]);
    if (!allowed)
        return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
    const searchParams = request.nextUrl.searchParams;
    const page = Number(searchParams.get("page")) || 1;
    const search = searchParams.get("search") || undefined;
    const data = {
        page,
        search
    }
    const result = await toolFilters.safeParseAsync(data);
    if(!result.success)
        return NextResponse.json({message: `Erro ao validar dados: ${result.error.message}`})
    const filters = result.data;
    const response = await getToolsByPagination(filters);
    if(response.status !== 200 && response.status !== 201){
        return NextResponse.json({
            message: response.data.message
        },{
            status: response.status
        })
    }
    return NextResponse.json(response.data);
}