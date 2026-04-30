import { addImageMarkDownSchema } from "@/domain/schemas/AddImageMarkDownSchema";
import { validateUserByRequest } from "@/services/server/auth-services";
import { addImageMarkDown } from "@/services/server/image-server";
import { NextRequest, NextResponse } from "next/server";

export async function POST(request: NextRequest) {
    const allowed = await validateUserByRequest(request, ["Administrador"])
    if (!allowed)
        return NextResponse.json({ message: "Unauthorized" }, { status: 401 })
    const formData = await request.formData();
    const data = {
        file: formData.get("file")
    }
    const result = await addImageMarkDownSchema.safeParseAsync(data)
    if (result.error) {
        return NextResponse.json({
            message: `Erro ao validar dados: ${result.error.message}`
        }, {
            status: 400
        })
    }
    const {file} = result.data;
    const response = await addImageMarkDown(file);
    if(response.status !== 200 && response.status !==201){
        return NextResponse.json({
            message: response.data.message
        },{
            status: response.status
        })
    }
    return NextResponse.json(response.data);
}
