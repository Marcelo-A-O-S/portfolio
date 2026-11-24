import { signOut } from "next-auth/react";

export async function logout() {
    try{
        const response = await fetch("/api/auth/logout",{
            method:"POST"
        });
        if(!response.ok)
            console.error("Logout backend failed:", response.status);
    }catch(error){
        console.error("Failed to communicate with AuthService:", error);
    }finally{
        await signOut({ callbackUrl: "/"})
    }
}