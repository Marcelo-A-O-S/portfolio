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
        const parsedData = {
            ...data,
            issuerDate: new Date(data.issuerDate)
        }
        const result = await certificateSchema.safeParseAsync(parsedData);
        if(!result.success){
            console.log(`Erro ao validar dados: ${result.error.message}`)
            return NextResponse.json({
                message: `Erro ao validar dados: ${result.error.message}`
            }, {
                status: 400
            });
        }
        const certificate = result.data;
        console.log("Dados do Certificado: ",certificate)
        console.log("Salvando certificado...")
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