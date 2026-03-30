import { postSchema } from "@/domain/schemas/PostSchema";
import { validateUserByRequest } from "@/services/server/auth-services";
import { deletePostService, getPostByIdService, updatePostService } from "@/services/server/post-services";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest, { params }: { params: Promise<{ Id: string }> }) {
    const allowed = await validateUserByRequest(request, ["Administrador"]);
    if (!allowed)
        return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
    const { Id } = await params;
    const response = await getPostByIdService(Id);
    if(response.status !== 200){
        return NextResponse.json({
            message: response.data.message
        },{
            status: response.data
        })
    }
    return NextResponse.json(response.data);
}
export async function PUT(request: NextRequest, {params }: { params: Promise<{Id: string}>}){
    const allowed = await validateUserByRequest(request, ["Administrador"]);
    if (!allowed)
        return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
    const { Id } = await params;
    const data = await request.json();
    const result = await postSchema.safeParseAsync(data);
    if(result.error){
        return NextResponse.json({
            message: `Erro ao validar dados: ${result.error.message}`
        },{
            status: 400
        });
    }
    const post = result.data;
    const response = await updatePostService(Id,post);
    if(response.status !== 200){
        return NextResponse.json({
            message: response.data.message
        },{
            status: response.data
        })
    }
    return NextResponse.json({ message: "Postagem atualizada com sucesso."})
}
export async function DELETE(request:NextRequest, { params }: { params: Promise<{Id: string}>}){
    const allowed = await validateUserByRequest(request, ["Administrador"]);
    if (!allowed)
        return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
    const { Id } = await params;
    const response = await deletePostService(Id);
    if(response.status !== 200){
        return NextResponse.json({
            message: response.data.message
        },{
            status: response.data
        })
    }
    return NextResponse.json({ message: "Postagem deletada com sucesso."})
}