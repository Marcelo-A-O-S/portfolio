"use client"
import DialogAccounts from "@/app/admin/users/components/dialog-accounts"
import { User } from "@/domain/types/User"
import { useModifyRole } from "@/hooks/useModifyRole"
import { MoreHorizontal } from "lucide-react"
import Link from "next/link"
import { Button } from "./ui/button"
import { DropdownMenu, DropdownMenuTrigger, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuSub, DropdownMenuSubTrigger, DropdownMenuPortal, DropdownMenuSubContent, DropdownMenuSeparator } from "./ui/dropdown-menu"
interface UserActionsProps {
    user: User
}
export function UserActions({ user }: UserActionsProps) {
    const { mutate } = useModifyRole()
    return (
        <>
            <DropdownMenu >
                <DropdownMenuTrigger asChild>
                    <div className="flex items-center justify-center gap-2 flex-row-reverse">
                        <Button variant="ghost" size="icon" className="rounded-full">
                            <MoreHorizontal className="h-4 w-4" />
                        </Button>
                    </div>
                </DropdownMenuTrigger>
                <DropdownMenuContent align="end" className="w-[200px]">
                    <DropdownMenuGroup>
                        <DropdownMenuItem
                            onSelect={(e) => e.preventDefault()}
                        >
                            <DialogAccounts socialAccounts={user.socialAccounts} />
                        </DropdownMenuItem>
                        <DropdownMenuSub>
                            <DropdownMenuSubTrigger>Modificar função</DropdownMenuSubTrigger>
                            <DropdownMenuPortal>
                                <DropdownMenuSubContent>
                                    <DropdownMenuItem onClick={() =>
                                        mutate({ userId: user.id, role: "Administrador" })
                                    }>Administrador</DropdownMenuItem>
                                    <DropdownMenuItem onClick={() =>
                                        mutate({ userId: user.id, role: "Client" })
                                    }>Client</DropdownMenuItem>
                                </DropdownMenuSubContent>
                            </DropdownMenuPortal>
                        </DropdownMenuSub>
                        <DropdownMenuItem>
                            <Link className="text-sm w-full" href={"/admin/users"}>Deletar usuário</Link>
                        </DropdownMenuItem>
                    </DropdownMenuGroup>
                    <DropdownMenuSeparator />
                    <DropdownMenuItem className="cursor-pointer" >
                        Banir usuário
                    </DropdownMenuItem>
                </DropdownMenuContent>
            </DropdownMenu>
        </>
    )
}