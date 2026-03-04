import { postSchema } from "@/domain/schemas/PostSchema";
import { addPostService } from "@/services/server/post-services";
import { NextRequest, NextResponse } from "next/server";

export async function POST(request: NextRequest){
    const data = await request.json();
    const result = await postSchema.safeParseAsync(data);
    if(result.error){
        return NextResponse.json({
            message: result.error.message
        },{
            status:400
        });
    }
    const post = result.data;
    const response = await addPostService(post);
    if(response.status !== 200 && response.status !== 201){
        return NextResponse.json({
            message: response.data.message
        },{
            status: response.status
        })
    }
    return NextResponse.json({
        message:"Post salvo com sucesso"
    })
}