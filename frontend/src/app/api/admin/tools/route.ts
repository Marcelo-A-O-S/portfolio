import { toolSchema } from "@/domain/schemas/ToolSchema";
import { handleApiError } from "@/lib/api-error";
import { validateUserByRequest } from "@/services/server/auth-services";
import { addToolService, getTools } from "@/services/server/tool-services";
import { NextRequest, NextResponse } from "next/server";

export async function POST(request: NextRequest) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
        const data = await request.json();
        const result = await toolSchema.safeParseAsync(data);
        if (!result.success) {
            console.dir(data, { depth: null });
            console.log(`Erro ao validar dados: ${result.error.message}`)
            return NextResponse.json({
                message: `Erro ao validar dados: ${result.error.message}`
            }, {
                status: 400
            })
        }
        const tool = result.data;
        console.log("Salvando ferramenta...")
        const response = await addToolService(tool);
        if (response.status !== 200 && response.status !== 201) {
            console.log(`Erro: ${response.data.message}`)
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            });
        }
        return NextResponse.json({ message: "Ferramenta salva com sucesso!" })
    } catch (error) {
        return handleApiError(error);
    }
}
export async function GET(request: NextRequest) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
        const response = await getTools();
        if (response.status !== 200 && response.status !== 201) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            });
        }
        return NextResponse.json(response.data);
    } catch (error) {
        return handleApiError(error);
    }
}