import { ReactNode } from "react"
import Navbar from "../Navbar"
import Footer from "../Footer"
interface ContainerProps {
    children : ReactNode
}
export default function Container({children}:ContainerProps){
    return(
        <>
        <Navbar/>
        {children}
        <Footer/>
        </>
    )
}