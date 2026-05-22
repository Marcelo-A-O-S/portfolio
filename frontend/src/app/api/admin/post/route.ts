import { postSchema } from "@/domain/schemas/PostSchema";
import { ApiErrorResponse } from "@/domain/types/ApiErrorResponse";
import { validateUserByRequest } from "@/services/server/auth-services";
import { addPostService, getPosts } from "@/services/server/post-services";
import axios from "axios";
import { NextRequest, NextResponse } from "next/server";

export async function POST(request: NextRequest) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
        const formData = await request.formData();
        const parsedData = {
            imgUrl: formData.get("imgUrl"),
            imgFile: formData.get("imgFile"),
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
        const response = await addPostService(post);
        if (response.status !== 200 && response.status !== 201) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            })
        }
        return NextResponse.json({
            message: "Post salvo com sucesso"
        })
    } catch (error) {
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
export async function GET(request: NextRequest) {
    const allowed = await validateUserByRequest(request, ["Administrador"]);
    if (!allowed)
        return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
    const response = await getPosts();
    if (response.status !== 200 && response.status !== 201) {
        return NextResponse.json({
            message: response.data.message
        }, {
            status: response.status
        });
    }
    return NextResponse.json(response.data);
}