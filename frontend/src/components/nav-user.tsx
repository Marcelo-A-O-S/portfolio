import { Session } from "next-auth";
import { Button } from "./ui/button";
import { Avatar, AvatarFallback, AvatarImage } from "./ui/avatar";
import { DropdownMenu, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuSeparator, DropdownMenuTrigger } from "./ui/dropdown-menu";
import { LogOutIcon } from "lucide-react";
import { signOut } from "next-auth/react";
import Link from "next/link";
type NavUserProps = {
    session: Session
}
export default function NavUser({ session }: NavUserProps) {
    return (
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
                            <p className="hidden md:flex text-sm font-semibold">{session.user.name!}</p>
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
                            <Link className="text-sm w-full" href={"/admin/languages"}>Linguagens</Link>
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
    )
}