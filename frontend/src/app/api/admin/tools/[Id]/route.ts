import { toolSchema } from "@/domain/schemas/ToolSchema";
import { ApiErrorResponse } from "@/domain/types/ApiErrorResponse";
import { validateUserByRequest } from "@/services/server/auth-services";
import { deleteToolByRouteService, getToolByIdService, updateToolService } from "@/services/server/tool-services";
import axios from "axios";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest, { params }: { params: Promise<{ Id: string }> }) {
    const allowed = await validateUserByRequest(request, ["Administrador", "Client"]);
    if (!allowed) {
        return NextResponse.json({ error: "Unauthorized" }, { status: 401 });
    }
    const { Id } = await params;
    const response = await getToolByIdService(Id);
    if (response.status !== 200 && response.status !== 201) {
        return NextResponse.json({
            message: response.data.message
        }, {
            status: response.status
        });
    }
    return NextResponse.json(response.data)

}
export async function PUT(request: NextRequest, { params }: { params: Promise<{ Id: string }> }) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador", "Client"]);
        if (!allowed) {
            return NextResponse.json({ error: "Unauthorized" }, { status: 401 });
        }
        const { Id } = await params;
        const formData = await request.formData();
        const parsedData = {
            imgUrl: formData.get("imgUrl") || undefined,
            imgFile: formData.get("imgFile") || undefined,
            status: formData.get("status"),
            categories: JSON.parse(formData.get("categories") as string),
            toolContents: JSON.parse(formData.get("toolContents") as string)
        };
        console.log(parsedData);
        const result = await toolSchema.safeParseAsync(parsedData);
        if (result.error) {
            return NextResponse.json({
                message: `Erro ao validar dados: ${result.error.message}`
            }, {
                status: 400
            });
        }
        const tool = result.data;
        const response = await updateToolService(Id, tool);
        if (response.status !== 200 && response.status !== 201) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            });
        }
        return NextResponse.json({ message: "Ferramenta atualizada com sucesso!" })
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
export async function DELETE(request: NextRequest, { params }: { params: Promise<{ Id: string }> }) {
    const allowed = await validateUserByRequest(request, ["Administrador", "Client"]);
    if (!allowed) {
        return NextResponse.json({ error: "Unauthorized" }, { status: 401 });
    }
    const { Id } = await params;
    const response = await deleteToolByRouteService(Id);
    if (response.status !== 200 && response.status !== 201) {
        return NextResponse.json({
            message: response.data.message
        }, {
            status: response.status
        });
    }
    return NextResponse.json({ message: "Ferramenta deletada com sucesso!" })
}