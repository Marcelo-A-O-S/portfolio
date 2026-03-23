import { ReactNode } from "react"
import { cn } from "@/lib/utils"
type MaxWidthWrapperProps = {
    className: string,
    children: ReactNode
}
export default function MaxWidthWrapper({children, className}: MaxWidthWrapperProps){
    return(
        <div className={cn('mx-auto max-w-7xl w-full my-12', className)}>
            {children}
        </div>
    )
}