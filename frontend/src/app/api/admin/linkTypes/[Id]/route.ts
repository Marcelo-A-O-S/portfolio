import { linkTypeSchema } from "@/domain/schemas/LinkTypeSchema";
import { ApiErrorResponse } from "@/domain/types/ApiErrorResponse";
import { handleApiError } from "@/lib/api-error";
import { validateUserByRequest } from "@/services/server/auth-services";
import { deleteLinkType, updateLinkType } from "@/services/server/link-type-services";
import axios from "axios";
import { NextRequest, NextResponse } from "next/server";

export async function PUT(request: NextRequest, { params }: { params: Promise<{ Id: string }> }) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
        const { Id } = await params;
        if(Id == null || Id == undefined){
            return NextResponse.json({
                message: `Identificador não informado.`
            }, {
                status: 400
            })
        }
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
        const response = await updateLinkType(Id, linkType);
        if (response.status !== 200 && response.status !== 201) {
            console.log(`Erro: ${response.data.message}`)
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            });
        }
        return NextResponse.json({ message: "Tipo de link atualizado com sucesso!" });
    } catch (error) {
        return handleApiError(error);
    }
}
export async function DELETE(request:NextRequest, { params }: { params: Promise<{ Id: string }> }){
    try{
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
        const { Id } = await params;
        if(Id == null || Id == undefined){
            return NextResponse.json({
                message: `Identificador não informado.`
            }, {
                status: 400
            })
        }
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
        const response = await deleteLinkType(Id, linkType);
        if (response.status !== 200 && response.status !== 201) {
            console.log(`Erro: ${response.data.message}`)
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            });
        }
        return NextResponse.json({ message: "Tipo de link atualizado com sucesso!" });
    }catch(error){
        return handleApiError(error);
    }
}