import { likeSchema } from "@/domain/schemas/LikeSchema";
import { handleApiError } from "@/lib/api-error";
import { validateUserByRequest } from "@/services/server/auth-services";
import { addLikePost, removeLikePost } from "@/services/server/post-services";
import { NextRequest, NextResponse } from "next/server";
export async function POST(request: NextRequest) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
        const data = await request.json();
        const result = await likeSchema.safeParseAsync(data);
        if (!result.success) {
            return NextResponse.json({
                message: `Erro ao validar dados: ${result.error.message}`
            }, {
                status: 400
            });
        }
        const like = result.data;
        console.log("Adicionando curtida...")
        const response = await addLikePost(like);
        if (response.status !== 200 && response.status !== 201) {
            console.log("Erro ao curtir postagem:", response.data.message);
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            });
        }
        console.log("Curtida adicionada com sucesso:", response.data);
        return NextResponse.json(response.data);
    } catch (error) {
        return handleApiError(error);
    }
}
export async function DELETE(request: NextRequest) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
        const data = await request.json();
        const result = await likeSchema.safeParseAsync(data);
        if (!result.success) {
            return NextResponse.json({
                message: `Erro ao validar dados: ${result.error.message}`
            }, {
                status: 400
            });
        }
        const like = result.data;
        console.log("Removendo curtida...")
        const response = await removeLikePost(like);
        if (response.status !== 200 && response.status !== 201) {
            console.log("Erro ao remover curtida da postagem:", response.data.message);
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            });
        }
        console.log("Curtida removida com sucesso:", response.data);
        return NextResponse.json(response.data);
    } catch (error) {
        return handleApiError(error);
    }
}