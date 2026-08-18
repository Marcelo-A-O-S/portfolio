import { commentSchema } from "@/domain/schemas/CommentSchema";
import { ApiErrorResponse } from "@/domain/types/ApiErrorResponse";
import { validateUserByRequest } from "@/services/server/auth-services";
import { hardRemovePostComment } from "@/services/server/post-services";
import axios from "axios";
import { NextRequest, NextResponse } from "next/server";

export async function DELETE(request: NextRequest, { params }: { params: Promise<{ Id: string }> }) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed) {
            return NextResponse.json({ error: "Unauthorized" }, { status: 401 });
        }
        const { Id } = await params;
        const data = await request.json();
        const result = await commentSchema.safeParseAsync(data);
        if (result.error) {
            return NextResponse.json({
                message: `Erro ao validar dados: ${result.error.message}`
            }, {
                status: 400
            });
        }
        const comment = result.data;
        const response = await hardRemovePostComment(Id, comment);
        if (response.status !== 200 && response.status !== 201) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            });
        }
        return NextResponse.json({ message: "Comentário removido com sucesso!" })
    } catch (error) {
        if (axios.isAxiosError<ApiErrorResponse>(error)) {
            console.log(error.response?.data);
            return NextResponse.json(
                {
                    message: error.response?.data?.message ?? "Erro no backend"
                },
                {
                    status: error.response?.status ?? 500
                }
            );
        }
    }
}