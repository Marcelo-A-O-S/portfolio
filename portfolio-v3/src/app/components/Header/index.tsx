"use client"
import Link from "next/link";
import { useState } from "react";
import { useSession } from "next-auth/react";
import Image from "next/image";
export default function Header() {
    const { data: userSession, status, update } = useSession()
    const [openMenu, setOpenMenu] = useState(false)
    const updateMenu = () => {
        setOpenMenu(!openMenu)
    }
    return (
        <>
            <nav className="container mx-auto flex items-center justify-between p-5">
                <div className="text-white font-bold text-xl">
                    <h1>Marcelo Augusto</h1>
                </div>
                <div>
                    <div className="md:hidden">
                        <button className="outline-none mobile-menu-button" onClick={updateMenu} >
                            <svg className="w-6 h-6 text-white" x-show="!showMenu" fill="none" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" viewBox="0 0 24 24" stroke="currentColor">
                                <path d="M4 6h16M4 12h16M4 18h16"></path>
                            </svg>
                        </button>
                    </div>
                    {userSession ? (
                        <>
                            <div className=" hidden  md:flex ">
                                <ul className="flex justify-items-center justify-center gap-2">
                                    <li><Link href="/" className="text-white cursor-pointer">Home</Link></li>
                                    <li><Link href="/about" className="text-white cursor-pointer">About</Link></li>
                                    <li><Link href="/projects" className="text-white cursor-pointer">Projects</Link></li>
                                    <li>
                                        <div className="flex">
                                            {userSession.user?.image && userSession.user?.name  ? (
                                                <>
                                                <p>{userSession.user?.name}</p>
                                                <Image className="rounded-xl" src={userSession.user?.image} alt={userSession.user?.name} width={100} height={100}/>
                                                </>
                                            ):(
                                                <></>
                                            )}
                                            
                                        </div>
                                    </li>
                                </ul>
                            </div>
                            <div className="fixed z-10 top-1/2 left-1/2 -translate-x-2/4 -translate-y-2/4  md:hidden ">
                                <ul className={`${openMenu ? "" : "hidden"} flex w-full h-full flex-col text-5xl justify-items-center justify-center gap-2 md:hidden `}>
                                    <li><Link href="/" className="text-white cursor-pointer">Home</Link></li>
                                    <li><Link href="/about" className="text-white cursor-pointer">About</Link></li>
                                    <li><Link href="/projects" className="text-white cursor-pointer">Projects</Link></li>
                                    <li><Link href="/login" className="text-white cursor-pointer">Login</Link></li>
                                </ul>
                            </div>
                        </>) : (
                        <>
                            <div className=" hidden  md:flex ">
                                <ul className="flex justify-items-center justify-center gap-2">
                                    <li><Link href="/" className="text-white cursor-pointer">Home</Link></li>
                                    <li><Link href="/about" className="text-white cursor-pointer">About</Link></li>
                                    <li><Link href="/projects" className="text-white cursor-pointer">Projects</Link></li>
                                    <li><Link href="/login" className="text-white cursor-pointer">Login</Link></li>
                                </ul>
                            </div>
                            <div className="fixed z-10 top-1/2 left-1/2 -translate-x-2/4 -translate-y-2/4  md:hidden ">
                                <ul className={`${openMenu ? "" : "hidden"} flex w-full h-full flex-col text-5xl justify-items-center justify-center gap-2 md:hidden `}>
                                    <li><Link href="/" className="text-white cursor-pointer">Home</Link></li>
                                    <li><Link href="/about" className="text-white cursor-pointer">About</Link></li>
                                    <li><Link href="/projects" className="text-white cursor-pointer">Projects</Link></li>
                                    <li><Link href="/login" className="text-white cursor-pointer">Login</Link></li>
                                </ul>
                            </div>
                        </>)
                    }

                </div>
            </nav>
        </>)
}