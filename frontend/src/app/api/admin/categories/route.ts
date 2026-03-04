import { categorySchema } from "@/domain/schemas/CategorySchema";
import { addCategoryService } from "@/services/server/category-services";
import { NextRequest, NextResponse } from "next/server";

export async function POST(request: NextRequest){
    const data = request.json();
    const result = await categorySchema.safeParseAsync(data);
    if(result.error){
        return NextResponse.json({
            message : `Erro ao validar dados: ${result.error.message}`
        },{
            status: 400
        });
    }
    const category = result.data;
    const response = await addCategoryService(category);
    if(response.status !== 200 && response.status !== 201 ){
        return NextResponse.json({
            message: response.data.message
        },{
            status: response.status
        });
    }
    return NextResponse.json({ message: "Categoria salva com sucesso."})
}