import { categorySchema } from "@/domain/schemas/CategorySchema";
import { validateUserByRequest } from "@/services/server/auth-services";
import { updateCategoryService, deleteCategoryByRouteService } from "@/services/server/category-services";
import axios from "axios";
import { NextRequest, NextResponse } from "next/server";
type ApiErrorResponse = {
    message?: string;
    statusCode?: number;
};
export async function PUT(request: NextRequest, { params }: { params: Promise<{ Id: string }> }) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed) {
            return NextResponse.json({ error: "Unauthorized" }, { status: 401 });
        }
        const { Id } = await params;
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
        const response = await updateCategoryService(Id, category);
        if (response.status !== 200 && response.status !== 201) {
            console.log(response.data);
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            });
        }
        return NextResponse.json({ message: "Categoria atualizada com sucesso!" })
    } catch (error) {
        if (axios.isAxiosError<ApiErrorResponse>(error)) {
            console.log("Erro backend:", error.response?.data);
            console.log("Erro backend:", error.response);
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
export async function DELETE(request: NextRequest, { params }: { params: Promise<{ Id: string }> }) {
    const allowed = await validateUserByRequest(request, ["Administrador", "Client"]);
    if (!allowed) {
        return NextResponse.json({ error: "Unauthorized" }, { status: 401 });
    }
    const { Id } = await params;
    const response = await deleteCategoryByRouteService(Id);
    if (response.status !== 200 && response.status !== 201) {
        return NextResponse.json({
            message: response.data.message
        }, {
            status: response.status
        });
    }
    return NextResponse.json({ message: "Categoria deletada com sucesso!" })
}