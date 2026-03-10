import { UsersFilters } from "@/domain/interfaces/UsersFilters";
import { validateUserByRequest } from "@/services/server/auth-services";
import { getUsersByPaginationService } from "@/services/server/user-services";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest){
    const allowed = await validateUserByRequest(request,["Administrador","Client"]);
    if(!allowed)
        return NextResponse.json({message: "Unauthorized"}, {status: 401});
    const searchParams = request.nextUrl.searchParams;
    const filters : UsersFilters = {
        page: Number(searchParams.get("page")) || 1,
        search: searchParams.get("search") || undefined,
        role: searchParams.get("role") || undefined,
        status: searchParams.get("status") || undefined,
    }
    const response = await getUsersByPaginationService(filters);;
    if(response.status !== 200 && response.status !== 201){
        return NextResponse.json({
            message: response.data.message
        },{
            status: response.status
        })
    }
    return NextResponse.json(response.data);
}