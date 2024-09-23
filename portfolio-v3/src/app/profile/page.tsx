'use client'
import { useSession } from "next-auth/react"
export default function ProfilePage() {
    const { data: userSession } = useSession()
    return (
        <>
            <h1>Hello {userSession?.user?.name}</h1>
        </>
    )
}