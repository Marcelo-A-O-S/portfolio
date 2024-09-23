'use client'
import React, { ReactNode } from "react"
import Header from "../Header"
import Footer from "../Footer"
import { SessionProvider } from "next-auth/react"
interface ContainerProps{
    children: ReactNode
}
export default function Container({children}: ContainerProps){
    return(<>
    <SessionProvider>
        <Header/>
        {children}
        <Footer/>
    </SessionProvider>
    </>)
}