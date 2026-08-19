import { commentFilters } from "@/domain/schemas/CommentFilters";
import { validateUserByRequest } from "@/services/server/auth-services";
import { getPostCommentsByPagination } from "@/services/server/post-services";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest) {
    const allowed = await validateUserByRequest(request, ["Administrador"]);
    if (!allowed)
        return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
    const searchParams = request.nextUrl.searchParams;
    const page = Number(searchParams.get("page")) || 1;
    const targetId = searchParams.get("targetId")
    const type = searchParams.get("type")
    const parsed = {
        page,
        targetId,
        type
    }
    console.log("Dados: ",parsed)
    const result = await commentFilters.safeParseAsync(parsed);
    if(!result.success){
        console.log(`Erro ao válidar dados: ${result.error.message}`)
        return NextResponse.json({
            message: `Erro ao válidar dados: ${result.error.message}`
        },{
            status: 400
        })
    }
    const filters = result.data;
    const response = await getPostCommentsByPagination(filters);
    if(response.status !== 200 && response.status !== 201){
        return NextResponse.json({
            message: response.data.message
        },{
            status: response.status
        })
    }
    return NextResponse.json(response.data);
}