import { languageSchema } from "@/domain/schemas/LanguageSchema";
import { validateUserByRequest } from "@/services/server/auth-services";
import { deleteLanguageService, updateLanguageService } from "@/services/server/language-services";
import { NextRequest, NextResponse } from "next/server";

export async function GET() {

}
export async function PUT(request: NextRequest, { params }: { params: Promise<{ Id: string }> }) {
    const allowed = await validateUserByRequest(request, ["Administrador"]);
    if (!allowed)
        return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
    const { Id } = await params;
    const data = await request.json();
    const result = await languageSchema.safeParseAsync(data);
    if (!result.success)
        return NextResponse.json({
            message: `Erro ao validar dados: ${result.error.message}`
        },{
            status: 400
        });
    const language = result.data;
    const response = await updateLanguageService(Id, language);
    if(response.status !== 200 && response.status !== 201){
        return NextResponse.json({
            message: response.data.message
        },{
            status: response.status
        });
    }
    return NextResponse.json(response.data);
}
export async function DELETE(request: NextRequest, {params }: { params: Promise<{Id: string}>}){
    const allowed = await validateUserByRequest(request, ["Administrador"]);
    if(!allowed)
        return NextResponse.json({ message: "Unauthorized"}, { status: 401 });
    const { Id } = await params;
    const response = await deleteLanguageService(Id);
    if(response.status !== 200 && response.status !== 201){
        return NextResponse.json({
            message: response.data.message
        },{
            status: response.status
        })
    }
    return NextResponse.json(response.data);
}