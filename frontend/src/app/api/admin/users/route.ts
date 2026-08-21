import { handleApiError } from "@/lib/api-error";
import { validateUserByRequest } from "@/services/server/auth-services";
import { getUsersService } from "@/services/server/user-services";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest) {
    try {
        const allowed = await validateUserByRequest(request, ["Administrador"]);
        if (!allowed)
            return NextResponse.json({ message: "Unauthorized" }, { status: 401 });
        const searchParams = request.nextUrl.searchParams;
        const search = searchParams.get("search") || "";
        const response = await getUsersService(search);
        if (response.status !== 200 && response.status !== 201) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            })
        }
        return NextResponse.json(response.data);
    } catch (error) {
        return handleApiError(error);
    }
}
