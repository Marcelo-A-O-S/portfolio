import { commentSchema } from "@/domain/schemas/CommentSchema";
import { handleApiError } from "@/lib/api-error";
import { validateUserByRequest } from "@/services/server/auth-services";
import { removePostCommentById, updatePostComment } from "@/services/server/post-services";
import { NextRequest, NextResponse } from "next/server";

export async function PUT(request: NextRequest, { params }: { params: Promise<{ Id: string }> }) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed) {
            return NextResponse.json({ error: "Unauthorized" }, { status: 401 });
        }
        const { Id } = await params;
        const data = await request.json();
        console.log("Dados: ",data)
        const result = await commentSchema.safeParseAsync(data);
        if (result.error) {
            console.log("Erro ao validar dados: ", result.error.message);
            return NextResponse.json({
                message: `Erro ao validar dados: ${result.error.message}`
            }, {
                status: 400
            });
        }
        const comment = result.data;
        const response = await updatePostComment(Id, comment);
        if (response.status !== 200 && response.status !== 201) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            });
        }
        return NextResponse.json({ message: "Comentário atualizado com sucesso!" })
    } catch (error) {
        return handleApiError(error);
    }
}
export async function DELETE(request: NextRequest,{ params }: { params: Promise<{ Id: string }> }){
    try{
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed) {
            return NextResponse.json({ error: "Unauthorized" }, { status: 401 });
        }
        const { Id } = await params;
        const data = await request.json();
        console.log("Dados: ",data)
        const result = await commentSchema.safeParseAsync(data);
        if (result.error) {
            console.log("Erro ao validar dados: ", result.error.message);
            return NextResponse.json({
                message: `Erro ao validar dados: ${result.error.message}`
            }, {
                status: 400
            });
        }
        const comment = result.data;
        const response = await removePostCommentById(Id, comment);
        if (response.status !== 200 && response.status !== 201) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            });
        }
        return NextResponse.json({ message: "Comentário deletado com sucesso!" })
    }catch(error){
        return handleApiError(error);
    }
}