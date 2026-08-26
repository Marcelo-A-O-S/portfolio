import { certificateFilters } from "@/domain/schemas/CertificateFilters";
import { certificateSchema } from "@/domain/schemas/CertificateSchema";
import { handleApiError } from "@/lib/api-error";
import { validateUserByRequest } from "@/services/server/auth-services";
import { getCertificatesByPagination } from "@/services/server/certificate-services";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
        const searchParams = request.nextUrl.searchParams;
        const page = Number(searchParams.get("page")) || 1;
        const search = searchParams.get("search") || undefined;
        const data = {
            page,
            search
        }
        const result = await certificateFilters.safeParseAsync(data);
        if (!result.success) {
            return NextResponse.json({
                message: `Error ao validar dados: ${result.error.message}`
            }, {
                status: 400
            });
        }
        const filters = result.data;
        const response = await getCertificatesByPagination(filters);
        if (response.status !== 200) {
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