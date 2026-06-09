import { likePostSchema } from "@/domain/schemas/LikePostSchema";
import { validateUserByRequest } from "@/services/server/auth-services";
import { addLikePost, removeLikePost } from "@/services/server/post-services";
import { NextRequest, NextResponse } from "next/server";
export async function POST(request: NextRequest) {
    const allowed = await validateUserByRequest(request, ["Administrador"]);
    if (!allowed)
        return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
    const data = await request.json();
    const result = await likePostSchema.safeParseAsync(data);
    if (!result.success) {
        return NextResponse.json({
            message: `Erro ao validar dados: ${result.error.message}`
        }, {
            status: 400
        });
    }
    const like = result.data;
    const response = await addLikePost(like);
    if (response.status !== 200 && response.status !== 201) {
        return NextResponse.json({
            message: response.data.message
        }, {
            status: response.status
        });
    }
    return NextResponse.json(response.data);
}
export async function DELETE(request: NextRequest) {
    const allowed = await validateUserByRequest(request, ["Administrador"]);
    if (!allowed)
        return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
    const data = await request.json();
    const result = await likePostSchema.safeParseAsync(data);
    if (!result.success) {
        return NextResponse.json({
            message: `Erro ao validar dados: ${result.error.message}`
        }, {
            status: 400
        });
    }
    const like = result.data;
    const response = await removeLikePost(like);
    if (response.status !== 200 && response.status !== 201) {
        return NextResponse.json({
            message: response.data.message
        }, {
            status: response.status
        });
    }
    return NextResponse.json(response.data);
}