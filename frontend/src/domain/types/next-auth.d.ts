import NextAuth from "next-auth";
import { JWT } from "next-auth/jwt";
import { Profile } from "next-auth";
declare module "next-auth" {
    interface Session {
        user: {
            id: string;
            name?: string;
            email?: string;
            image?: string;
            username?: string;
            provider?: string;
        }
    }
}
declare module "next-auth/jwt" {
    interface JWT{
        userId?: string;
        username?: string;
        provider?:string;
    }
}
declare module "next-auth"{
    interface Profile{
        login?: string;
        avatar_url?: string;
        created_at?: string;
    }
}
declare module "next-auth"{
    interface User{
        username?: string;
        name?:string;
        email?:string;
        image?:string;
    }
}
