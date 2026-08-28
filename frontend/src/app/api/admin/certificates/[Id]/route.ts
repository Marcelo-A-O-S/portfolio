import { certificateSchema } from "@/domain/schemas/CertificateSchema";
import { handleApiError } from "@/lib/api-error";
import { validateUserByRequest } from "@/services/server/auth-services";
import { deleteCertificateByRouteService, deleteCertificateService, getCertificateByIdService, updateCertificateService } from "@/services/server/certificate-services";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest, { params }: { params: Promise<{ Id: string }> }) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
        const { Id } = await params;
        if (Id == undefined || Id == null) {
            return NextResponse.json({
                message: `Identificador não informado!`
            }, {
                status: 400
            });
        }
        const response = await getCertificateByIdService(Id);
        if (response.status !== 200) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            })
        }
        return NextResponse.json(response.data);
    } catch (error) {
        return handleApiError(error);
    }
}
export async function PUT(request: NextRequest, { params }: { params: Promise<{ Id: string }> }) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
        const { Id } = await params;
        if (Id == undefined || Id == null) {
            return NextResponse.json({
                message: `Identificador não informado!`
            }, {
                status: 400
            });
        }
        const data = await request.json();
        const result = await certificateSchema.safeParseAsync(data);
        if (!result.success) {
            return NextResponse.json({
                message: `Erro ao validar dados: ${result.error.message}`
            }, {
                status: 400
            });
        }
        const certificate = result.data;
        const response = await updateCertificateService(Id, certificate);
        if (response.status !== 200) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            })
        }
        return NextResponse.json({ message: "Certificado atualizado com sucesso." })
    } catch (error) {
        return handleApiError(error);
    }
}
export async function DELETE(request: NextRequest, { params }: { params: Promise<{ Id: string }> }) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
        const { Id } = await params;
        if (Id == undefined || Id == null) {
            return NextResponse.json({
                message: `Identificador não informado!`
            }, {
                status: 400
            });
        }
        // const data = await request.json();
        // console.log("Dados: ",data);
        // const result = await certificateSchema.safeParseAsync(data);
        // if (!result.success) {
        //     console.log(`Erro ao validar dados: ${result.error.message}`)
        //     return NextResponse.json({
        //         message: `Erro ao validar dados: ${result.error.message}`
        //     }, {
        //         status: 400
        //     });
        // }
        // const certificate = result.data;
        const response = await deleteCertificateByRouteService(Id);
        if (response.status !== 200) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            })
        }
        return NextResponse.json({ message: "Certificado deletado com sucesso." })
    } catch (error) {
        return handleApiError(error);
    }
}