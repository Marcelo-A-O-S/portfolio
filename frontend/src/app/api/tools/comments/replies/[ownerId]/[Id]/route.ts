import { commentSchema } from "@/domain/schemas/CommentSchema";
import { ApiErrorResponse } from "@/domain/types/ApiErrorResponse";
import { validateUserByRequest } from "@/services/server/auth-services";
import { deleteToolReply, updateToolReply } from "@/services/server/tool-services";
import axios from "axios";
import { NextRequest, NextResponse } from "next/server";
export async function PUT(request: NextRequest, { params }: { params: Promise<{ ownerId: string, Id: string }> }) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador", "Moderator", "Client"]);
        if (!allowed) {
            return NextResponse.json({ error: "Unauthorized" }, { status: 401 });
        }
        const { ownerId, Id } = await params;
        if (Id == undefined || Id == null) {
            return NextResponse.json({
                message: "Identificador da resposta não informado!"
            }, {
                status: 400
            })
        }
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
        const response = await updateToolReply(ownerId, Id, reply);
        if (response.status !== 200 && response.status !== 201) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            });
        }
        return NextResponse.json({ message: "Resposta atualizada com sucesso!" })
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
export async function DELETE(request: NextRequest, { params }: { params: Promise<{ ownerId: string, Id: string }> }) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador", "Moderator", "Client"]);
        if (!allowed) {
            return NextResponse.json({ error: "Unauthorized" }, { status: 401 });
        }
        const { ownerId, Id } = await params;
        if (Id == undefined || Id == null) {
            return NextResponse.json({
                message: "Server Side: Identificador da resposta não informado!"
            }, {
                status: 400
            })
        }
        const data = await request.json();
        const result = await commentSchema.safeParseAsync(data);
        if (result.error) {

            return NextResponse.json({
                message: `Erro ao validar dados: ${result.error.message}`
            }, {
                status: 400
            });
        }
        const reply = result.data;
        const response = await deleteToolReply(ownerId, Id, reply);
        if (response.status !== 200 && response.status !== 201) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            });
        }
        return NextResponse.json({ message: "Resposta deletada com sucesso!" })
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