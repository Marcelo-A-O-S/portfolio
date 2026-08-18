import { linkSchema } from "@/domain/schemas/LinkSchema";
import { ApiErrorResponse } from "@/domain/types/ApiErrorResponse";
import { validateUserByRequest } from "@/services/server/auth-services";
import { addLinkPost } from "@/services/server/post-services";
import axios from "axios";
import { NextRequest, NextResponse } from "next/server";

export async function POST(request: NextRequest) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
        const data = await request.json();
        const result = await linkSchema.safeParseAsync(data);
        if(!result.success){
            return NextResponse.json({
                message: `Erro ao validar dados: ${result.error.message}`
            },{
                status: 400
            })
        }
        const link = result.data;
        const response = await addLinkPost(link);
        if (response.status !== 200 && response.status !== 201) {
            console.log(`Erro: ${response.data.message}`)
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            });
        }
        return NextResponse.json({ message: "Link vinculado a ferramenta com sucesso!" })
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