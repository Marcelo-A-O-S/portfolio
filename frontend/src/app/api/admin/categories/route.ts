import { categorySchema } from "@/domain/schemas/CategorySchema";
import { validateUserByRequest } from "@/services/server/auth-services";
import { addCategoryService } from "@/services/server/category-services";
import axios from "axios";
import { NextRequest, NextResponse } from "next/server";
type ApiErrorResponse = {
    message?: string;
    statusCode?: number;
};
export async function POST(request: NextRequest) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 })
        const data = await request.json();
        const result = await categorySchema.safeParseAsync(data);
        if (result.error) {
            console.log("Erro validação: ", result.error.message)
            return NextResponse.json({
                message: `Erro ao validar dados: ${result.error.message}`
            }, {
                status: 400
            });
        }
        const category = result.data;
        console.log(data);
        const response = await addCategoryService(category);
        console.log("Erro backend: ", response.data.message)
        if (response.status !== 200 && response.status !== 201) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            });
        }
        return NextResponse.json({ message: "Categoria salva com sucesso." })
    } catch (error: unknown) {
        if (axios.isAxiosError<ApiErrorResponse>(error)) {
            console.log("Erro backend:", error.response?.data);

            return NextResponse.json(
                {
                    message: error.response?.data?.message ?? "Erro no backend"
                },
                {
                    status: error.response?.status ?? 500
                }
            );
        }
    }

}
