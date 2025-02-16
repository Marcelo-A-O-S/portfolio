'use client'
import React, { ReactNode } from "react"
import Header from "../Header"
import Footer from "../Footer"
import { SessionProvider } from "next-auth/react"
import AdminProvider from "@/contexts/AdminContext"
interface ContainerProps{
    children: ReactNode
}
export default function Container({children}: ContainerProps){
    return(<>
    <SessionProvider>
        <AdminProvider>
            <Header/>
            {children}
            <Footer/>
        </AdminProvider>
    </SessionProvider>
    </>)
}