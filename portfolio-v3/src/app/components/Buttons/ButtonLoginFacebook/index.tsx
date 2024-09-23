'use client'
import { signIn } from "next-auth/react"
export default function ButtonLoginFacebook() {
    const handleLogin =()=>{
        signIn('facebook')
    }
    return (
        <>
            <button onClick={handleLogin} className="flex border-2 p-4 rounded-xl gap-2">
                <svg className="w-6 h-6 text-gray-800 dark:text-white" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" viewBox="0 0 24 24">
                    <path fillRule="evenodd" d="M13.135 6H15V3h-1.865a4.147 4.147 0 0 0-4.142 4.142V9H7v3h2v9.938h3V12h2.021l.592-3H12V6.591A.6.6 0 0 1 12.592 6h.543Z" clipRule="evenodd" />
                </svg>
                <p>Login with Facebook</p>
            </button>
        </>
    )
}