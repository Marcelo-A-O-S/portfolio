import { validateUserByRequest } from "@/services/server/auth-services";
import { modifyRoleService } from "@/services/server/user-services";
import { NextRequest, NextResponse } from "next/server";
export async function PATCH(request: NextRequest, { params }: { params: Promise<{ Id: string }> }) {
    const allowed = await validateUserByRequest(request,["Administrador","Client"]);
    if(!allowed)
        return NextResponse.json({message: "Unauthorized"}, {status: 401});
    const data = await request.json();
    const { Id } = await params;
    if(!data)
        return NextResponse.json({message:"A função é obrigatório."}, {status:400});
    if(!Id)
        return NextResponse.json({message:"O identificador de usuário é obrigatório."}, {status:400});
    const response = await modifyRoleService(Id, data);
    if(response.status !== 200 && response.status !== 201){
        return NextResponse.json({
            message: response.data.message
        },{
            status: response.status
        })
    }
    return NextResponse.json(response.data);
}