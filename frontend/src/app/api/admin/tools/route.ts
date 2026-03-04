import { toolSchema } from "@/domain/schemas/ToolSchema";
import { addToolService } from "@/services/server/tool-services";
import { NextRequest, NextResponse } from "next/server";

export async function POST(request:NextRequest){
    const data = request.json();
    const result = await toolSchema.safeParseAsync(data);
    if(result.error){
        return NextResponse.json({
            message: `Erro ao validar dados: ${result.error.message}`
        },{
            status: 400
        });
    }
    const tool = result.data;
    const response = await addToolService(tool);
    if(response.status !== 200 && response.status !== 201){
        return NextResponse.json({
            message: response.data.message
        },{
            status: response.status
        });
    }
    return NextResponse.json({ message: "Ferramenta salva com sucesso!"})
}