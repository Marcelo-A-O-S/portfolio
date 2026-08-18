import { categorySchema } from "@/domain/schemas/CategorySchema";
import { ApiErrorResponse } from "@/domain/types/ApiErrorResponse";
import { handleApiError } from "@/lib/api-error";
import { validateUserByRequest } from "@/services/server/auth-services";
import { addCategoryService, getCategories, getCategoriesByLanguage } from "@/services/server/category-services";
import axios from "axios";
import { NextRequest, NextResponse } from "next/server";
export async function POST(request: NextRequest) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
        const data = await request.json();
        const result = await categorySchema.safeParseAsync(data);
        if (result.error) {
            return NextResponse.json({
                message: `Erro ao validar dados: ${result.error.message}`
            }, {
                status: 400
            });
        }
        const category = result.data;
        const response = await addCategoryService(category);
        if (response.status !== 200 && response.status !== 201) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            });
        }
        return NextResponse.json({ message: "Categoria salva com sucesso." })
    } catch (error: unknown) {
        return handleApiError(error);
    }
}
export async function GET(request: NextRequest) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
        const searchParams = request.nextUrl.searchParams;
        const language = searchParams.get("language");
        let response = null;
        if (language) {
            response = await getCategoriesByLanguage(language);
        } else {
            response = await getCategories();
        }
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
