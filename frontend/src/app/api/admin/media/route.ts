import { mediaRequestSchema, MediaSchema } from "@/domain/schemas/MediaSchema";
import { ApiErrorResponse } from "@/domain/types/ApiErrorResponse";
import { validateUserByRequest } from "@/services/server/auth-services";
import { addMediaService } from "@/services/server/media-services";
import axios from "axios";
import { NextRequest, NextResponse } from "next/server";
export async function POST(request: NextRequest) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 })
        const formData = await request.formData();
        const parsedData  = {
            file: formData.get("file"),
            ownerType: formData.get("ownerType"),
            ownerId: formData.get("ownerId") || undefined
        }
        const result = await mediaRequestSchema.safeParseAsync(parsedData);
        if (result.error) {
            console.log(`Erro ao validar dados: ${result.error.message}`)
            return NextResponse.json({
                message: `Erro ao validar dados: ${result.error.message}`
            }, {
                status: 400
            });
        }
        const mediaRequest = result.data;
        const response = await addMediaService(mediaRequest);
        if (response.status !== 200 && response.status !== 201) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            })
        }
        return NextResponse.json(response.data)
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