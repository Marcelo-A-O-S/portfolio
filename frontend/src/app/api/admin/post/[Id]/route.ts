import { postSchema } from "@/domain/schemas/PostSchema";
import { ApiErrorResponse } from "@/domain/types/ApiErrorResponse";
import { validateUserByRequest } from "@/services/server/auth-services";
import { deletePostByRouteService, getPostByIdService, updatePostService } from "@/services/server/post-services";
import axios from "axios";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest, { params }: { params: Promise<{ Id: string }> }) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
        const { Id } = await params;
        const response = await getPostByIdService(Id);
        if (response.status !== 200) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.data
            })
        }
        return NextResponse.json(response.data);
    }catch (error) {
        if (axios.isAxiosError<ApiErrorResponse>(error)) {
            console.log(error.response?.data);
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
export async function PUT(request: NextRequest, { params }: { params: Promise<{ Id: string }> }) {
    const allowed = await validateUserByRequest(request, ["Administrador"]);
    if (!allowed)
        return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
    const { Id } = await params;
    const formData = await request.formData();
    const parsedData = {
        imgUrl: formData.get("imgUrl") || undefined,
        imgFile: formData.get("imgFile") || undefined,
        status: formData.get("status"),
        categories: JSON.parse(formData.get("categories") as string),
        tools: JSON.parse(formData.get("tools") as string),
        postContents: JSON.parse(formData.get("postContents") as string)
    };
    const result = await postSchema.safeParseAsync(parsedData);
    if (result.error) {
        return NextResponse.json({
            message: `Erro ao validar dados: ${result.error.message}`
        }, {
            status: 400
        });
    }
    const post = result.data;
    const response = await updatePostService(Id, post);
    if (response.status !== 200) {
        return NextResponse.json({
            message: response.data.message
        }, {
            status: response.data
        })
    }
    return NextResponse.json({ message: "Postagem atualizada com sucesso." })
}
export async function DELETE(request: NextRequest, { params }: { params: Promise<{ Id: string }> }) {
    const allowed = await validateUserByRequest(request, ["Administrador"]);
    if (!allowed)
        return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
    const { Id } = await params;
    const response = await deletePostByRouteService(Id);
    if (response.status !== 200) {
        return NextResponse.json({
            message: response.data.message
        }, {
            status: response.data
        })
    }
    return NextResponse.json({ message: "Postagem deletada com sucesso." })
}