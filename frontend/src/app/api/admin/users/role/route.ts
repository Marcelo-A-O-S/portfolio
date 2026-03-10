import { NextRequest } from "next/server";

export async function GET(request: NextRequest){

}
export async function PATCH(request:NextRequest){
    const data = await request.json();
}
