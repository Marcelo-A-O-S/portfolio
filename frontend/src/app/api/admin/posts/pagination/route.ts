import { postsFilters } from "@/domain/schemas/PostsFilters";
import { validateUserByRequest } from "@/services/server/auth-services";
import { getPostsByPagination } from "@/services/server/post-services";
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
    const result = await postsFilters.safeParseAsync(data);
    if(result.error){
        return NextResponse.json({
            message: `Error ao validar dados: ${result.error.message}`
        },{
            status: 400
        });
    }
    const filters = result.data;
    const response = await getPostsByPagination(filters);
    if(response.status !== 200){
        return NextResponse.json({
            message: response.data.message
        },{
            status: response.status
        });
    }
    return NextResponse.json(response.data);
}