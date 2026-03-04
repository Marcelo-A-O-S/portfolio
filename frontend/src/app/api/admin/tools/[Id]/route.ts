import { toolSchema } from "@/domain/schemas/ToolSchema";
import { validateUserByRequest } from "@/services/server/auth-services";
import { deleteToolByRouteService, updateToolService } from "@/services/server/tool-services";
import { NextRequest, NextResponse } from "next/server";

export async function GET() {

}
export async function PUT(request: NextRequest, { params }: { params: Promise<{ Id: string }> }) {
    const allowed = await validateUserByRequest(request, ["Administrador", "Client"]);
    if (!allowed) {
        return NextResponse.json({ error: "Unauthorized" }, { status: 401 });
    }
    const { Id } = await params;
    const data = await request.json();
    const result = await toolSchema.safeParseAsync(data);
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