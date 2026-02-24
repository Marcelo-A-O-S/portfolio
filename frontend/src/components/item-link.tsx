import Link from "next/link";
import type { MouseEvent } from "react";
import { useEffect, useRef } from "react"
import gsap from "gsap";
export type LinkProps = {
    imgString: string,
    linkName: string,
    pathName: string,
    isOpen?: boolean,
    updateImage?:(e: MouseEvent<HTMLAnchorElement>) => void,
    onPush?: () =>void
}
export default function ItemLink({imgString,index,linkName,isOpen,pathName,updateImage, onPush}: LinkProps & { index:number}){
    const elementRef = useRef<HTMLDivElement | null>(null);
    useEffect(()=>{
        if(!elementRef.current || !isOpen) return;
        gsap.fromTo(elementRef.current,{
            opacity: 0,
            y:50
        },{
            stagger: 1,
            opacity: 1,
            y: 0,
            duration: 1,
            ease: "power4.inOut",
            delay: index * 0.6
        })
    },[isOpen])
    return(
    <>
    <div ref={elementRef} className="link pb-1.5 relative inline-block group overflow-hidden">
        <Link href={pathName} onClick={onPush} onMouseOver={updateImage} className="relative inline-block m-0 will-change-transform transition-colors duration-500 text-5xl font-medium tracking-tight z-20" data-image={imgString}>{linkName}</Link>
        <span className="absolute inset-0 bg-red-700 z-10 origin-left scale-x-0 transition-transform duration-500 group-hover:scale-x-100"></span>
    </div>
    </>
    )
}