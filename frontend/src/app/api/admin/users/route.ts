import { validateUserByRequest } from "@/services/server/auth-services";
import { getUsersByPaginationService, getUsersService } from "@/services/server/user-services";
import { NextRequest, NextResponse } from "next/server";

export async function POST(){
    
}
export async function GET(request: NextRequest){
    const allowed = await validateUserByRequest(request,["Administrador","Client"]);
    if(!allowed)
        return NextResponse.json({message: "Unauthorized"}, {status: 401});
    const searchParams = request.nextUrl.searchParams;
    const page = searchParams.get("page");
    let response = null;
    if(page){
        response = await getUsersByPaginationService(parseInt(page))
    }else{
        response = await getUsersService();
    }
    if(response.status !== 200 && response.status !== 201){
        console.log(response.data);
        return NextResponse.json({
            message: response.data.message
        },{
            status: response.status
        })
    }
    console.log(response.data);
    return NextResponse.json(response.data);
}