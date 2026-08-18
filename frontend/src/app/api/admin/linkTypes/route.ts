import { linkTypeSchema } from "@/domain/schemas/LinkTypeSchema";
import { handleApiError } from "@/lib/api-error";
import { validateUserByRequest } from "@/services/server/auth-services";
import { addLinkType, getLinkTypes } from "@/services/server/link-type-services";
import { NextRequest, NextResponse } from "next/server";
export async function POST(request: NextRequest) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
        const data = await request.json();
        const result = await linkTypeSchema.safeParseAsync(data);
        if (!result.success) {
            return NextResponse.json({
                message: `Erro ao validar dados: ${result.error.message}`
            }, {
                status: 400
            })
        }
        const linkType = result.data;
        const response = await addLinkType(linkType);
        if (response.status !== 200 && response.status !== 201) {
            console.log(`Erro: ${response.data.message}`)
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            });
        }
        return NextResponse.json({ message: "Tipo de link salvo com sucesso!" });
    } catch(error) {
        return handleApiError(error);
    }
}
export async function GET(request: NextRequest){
    try{
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
        console.log("Buscando os tipos de link ...")
        const response = await getLinkTypes();
        if (response.status !== 200 && response.status !== 201) {
            console.log(`Erro: ${response.data.message}`)
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            });
        }
        return NextResponse.json(response.data);
    }catch(error){
        return handleApiError(error);
    }
}