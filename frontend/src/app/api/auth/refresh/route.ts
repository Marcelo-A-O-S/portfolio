import { refreshAsync } from "@/services/server/auth-services";
import { getToken } from "next-auth/jwt";
import { cookies } from "next/headers";
import { NextRequest } from "next/server";
const authSecret = process.env.NEXTAUTH_SECRET!;
export async function GET(request: NextRequest){
}