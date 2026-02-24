import { signOut } from "next-auth/react";

export async function logout() {
    const response = await fetch("/api/auth/logout",{
        method:"POST"
    });
    if(!response.ok)
        console.error("Logout backend failed")
    await signOut({ callbackUrl: "/"})
}