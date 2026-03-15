import { NextRequest, NextResponse } from "next/server";
import { getToken } from "next-auth/jwt";
const pathPublics = [
    "/",
    "/projects",
    "/certificates",
    "/about",
    "/api/auth/logout",
    "/api/auth/refresh",
    "/api/auth/session",
    "/api/auth/signin",
    "/api/auth/callback",
    "/api/auth/providers",
    "/api/auth/csrf"
]
const roleProtectedRoutes: Record<string, string[]> = {
    "/api/admin/post":["Administrador"],
    "/admin/users": ["Administrador","Client"],
    "/admin/tools/create": ["Administrador"],
    "/admin/tools": ["Administrador"],
    "/admin/categories": ["Administrador"],
    "/admin/projects/create": ["Administrador"],
    "/admin/projects": ["Administrador"],
    "/admin/dashboard": ["Administrador"],
    "/admin/certificates": ["Administrador"],
    "/admin": ["Administrador"],
}
export async function middleware(request: NextRequest) {
    const { pathname } = request.nextUrl;

    if (pathPublics.includes(pathname)) {
        return NextResponse.next();
    }
    const token = await getToken({ req: request, secret: process.env.NEXTAUTH_SECRET! });
    if (!token || !token.role) {
        const url = request.nextUrl.clone();
        url.pathname = "/";
        return NextResponse.redirect(url);
    }
    for (const routePrefix in roleProtectedRoutes) {
        if (pathname.startsWith(routePrefix)) {
            const allowedRoles = roleProtectedRoutes[routePrefix];
            if (!allowedRoles.includes(token.role)) {
                const url = request.nextUrl.clone();
                url.pathname = "/";
                return NextResponse.redirect(url);
            }
            break;
        }
    }
    return NextResponse.next();
}
export const config = {
    matcher: [
        // "/((?!_next|favicon.ico|assets|sounds).*)",
        "/admin/:path*",
        "/api/admin/:path*"
        
    ]
}