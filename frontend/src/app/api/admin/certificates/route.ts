import { certificateSchema } from "@/domain/schemas/CertificateSchema";
import { handleApiError } from "@/lib/api-error";
import { validateUserByRequest } from "@/services/server/auth-services";
import { addCertificateService } from "@/services/server/certificate-services";
import { NextRequest, NextResponse } from "next/server";

export async function POST(request: NextRequest) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
        const data = await request.json();
        const result = await certificateSchema.safeParseAsync(data);
        if(!result.success){
            return NextResponse.json({
                message: `Erro ao validar dados: ${result.error.message}`
            }, {
                status: 400
            });
        }
        const certificate = result.data;
        const response = await addCertificateService(certificate);
        if (response.status !== 200 && response.status !== 201) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            })
        }
        return NextResponse.json({
            message: "Certificado salvo com sucesso"
        })
    } catch (error) {
        return handleApiError(error);
    }
}