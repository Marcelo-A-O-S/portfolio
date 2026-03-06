"use client"
import { Github, Instagram, Linkedin, LogOutIcon, StickyNote } from "lucide-react";
import { useEffect, useRef, useState } from "react"
import { FaGoogle } from "react-icons/fa6";
import { signIn, useSession, signOut } from "next-auth/react";
import Image from "next/image";
import gsap from "gsap";
import Link from "next/link";
import { DropdownMenu, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuSeparator, DropdownMenuTrigger } from "./ui/dropdown-menu";
import { Button } from "./ui/button";
import { Avatar, AvatarFallback, AvatarImage } from "./ui/avatar";
export default function LoginForm() {
    const { data: session } = useSession();
    const dialogRef = useRef<HTMLDialogElement | null>(null)
    const [isOpen, setIsOpen] = useState(false);
    const [openDropdown, setOpenDropdown] = useState(false);
    useEffect(() => {
        if (!dialogRef.current) return;
        if (isOpen) {
            dialogRef.current.showModal();
        } else {
            dialogRef.current.close();
        }
    }, [isOpen])
    return (
        <>
            <dialog ref={dialogRef} className="hidden open:flex fixed left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2  items-center justify-center bg-transparent">
                <div className="relative h-full w-full border-2 rounded-2xl
            border-primary dark:bg-black
            ">
                    <div className="flex flex-col gap-3 justify-center p-5 md:p-10">
                        <div className="flex flex-col gap-1">
                            <h1 className="text-lg font-semibold">Conecte-se para interagir</h1>
                            <hr />
                            <p className="">Acesse para reagir as postagens e ter outras vantagens.</p>
                            <hr />
                        </div>
                        <div className="flex flex-col gap-1">
                            <button onClick={() => signIn("github")} className="border-2 rounded-sm border-primary flex items-center justify-center p-1 gap-2 cursor-pointer"><Github />Github</button>
                            <button onClick={() => signIn("linkedin")} className="border-2 rounded-sm border-primary flex items-center justify-center p-1 gap-2 cursor-pointer"><Linkedin />Linkdlen</button>
                            <button onClick={() => signIn("google")} className="border-2 rounded-sm border-primary flex items-center justify-center p-1 gap-2 cursor-pointer"><FaGoogle />Google</button>
                        </div>
                        <div className="flex justify-end">
                            <hr />
                            <button onClick={() => setIsOpen(false)} className="border-2 rounded-sm border-primary flex items-center justify-center py-1 px-2 cursor-pointer">Fechar</button>
                        </div>
                    </div>
                </div>
            </dialog>
            {!session ?
                <button onClick={() => setIsOpen(true)} className="border-2 border-white rounded-sm py-1 px-2 font-semibold cursor-pointer">Login</button> :
                <>
                    <DropdownMenu >
                        <DropdownMenuTrigger asChild>
                            <div className="flex items-center justify-center gap-2 flex-row-reverse">
                                <Button variant="ghost" size="icon" className="rounded-full">
                                    <Avatar>
                                        <AvatarImage src={session.user.image!} alt={session.user.name!} />
                                        <AvatarFallback>LR</AvatarFallback>
                                    </Avatar>
                                </Button>
                                <div>
                                    <p className="hidden md:flex text-sm font-semibold">{session.user.username!}</p>
                                </div>
                            </div>
                        </DropdownMenuTrigger>
                        <DropdownMenuContent align="end" className="w-[200px]">
                            <DropdownMenuGroup>
                                <DropdownMenuItem>
                                    <Link className="text-sm w-full" href={"/admin/dashboard"}>Gerenciador</Link>
                                </DropdownMenuItem>
                                <DropdownMenuItem>
                                    <Link className="text-sm w-full" href={"/admin/projects"}>Projetos</Link>
                                </DropdownMenuItem>
                                <DropdownMenuItem>
                                    <Link className="text-sm w-full" href={"/admin/certificates"}>Certificados</Link>
                                </DropdownMenuItem>
                                <DropdownMenuItem>
                                    <Link className="text-sm w-full" href={"/admin/tools"}>Ferramentas</Link>
                                </DropdownMenuItem>
                                <DropdownMenuItem>
                                    <Link className="text-sm w-full" href={"/admin/categories"}>Categorias</Link>
                                </DropdownMenuItem>
                                <DropdownMenuItem>
                                    <Link className="text-sm w-full" href={"/admin/users"}>Usuários</Link>
                                </DropdownMenuItem>
                            </DropdownMenuGroup>
                            <DropdownMenuSeparator />
                            <DropdownMenuItem className="cursor-pointer" onClick={() => signOut()}>
                                <LogOutIcon />
                                Sign Out
                            </DropdownMenuItem>
                        </DropdownMenuContent>
                    </DropdownMenu>
                </>
            }
        </>
    )
}