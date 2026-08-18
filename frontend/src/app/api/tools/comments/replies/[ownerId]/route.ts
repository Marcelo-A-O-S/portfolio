import { commentSchema } from "@/domain/schemas/CommentSchema";
import { handleApiError } from "@/lib/api-error";
import { validateUserByRequest } from "@/services/server/auth-services";
import { addToolReply } from "@/services/server/tool-services";
import { NextRequest, NextResponse } from "next/server";
export async function POST(request: NextRequest, { params }: { params: Promise<{ ownerId: string }> }) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador","Moderator","Client"]);
        if (!allowed) {
            return NextResponse.json({ error: "Unauthorized" }, { status: 401 });
        }
        const { ownerId } = await params;
        const data = await request.json();
        const result = await commentSchema.safeParseAsync(data);
        if (result.error) {
            console.log("Erro ao validar dados: ", result.error.message);
            return NextResponse.json({
                message: `Erro ao validar dados: ${result.error.message}`
            }, {
                status: 400
            });
        }
        const reply = result.data;
        const response = await addToolReply(ownerId, reply);
        if (response.status !== 200 && response.status !== 201) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            });
        }
        return NextResponse.json({ message: "Resposta adicionada com sucesso!" })
    } catch (error) {
        return handleApiError(error);
    }
}