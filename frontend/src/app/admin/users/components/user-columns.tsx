import { User } from "@/domain/types/User"
import { ColumnDef } from "@tanstack/react-table"
import { Button } from "@/components/ui/button"
import { DropdownMenuTrigger, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuSeparator, DropdownMenu, DropdownMenuSub, DropdownMenuSubTrigger, DropdownMenuPortal, DropdownMenuSubContent } from "@/components/ui/dropdown-menu"
import { LogOutIcon, MoreHorizontal } from "lucide-react"
import Link from "next/link"
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";
import DialogAccounts from "./dialog-accounts"
export const getUsersColumns = () => {
    const columns: ColumnDef<User>[] = [
        {
            accessorKey: "name",
            header: "Nome"
        },
        {
            accessorKey: "email",
            header: "Email"
        },
        {
            accessorKey: "role",
            header: "Função"
        },
        {
            accessorKey: "status",
            header: "Status"
        },
        {
            accessorKey: "createdAt",
            header: "Data de registro",
            cell: ({ getValue }) => {
                const createdAt = getValue() as string
                const formated = format(new Date(createdAt),"dd/MM/yyyy",{locale:ptBR});
                return (
                    <>
                        <div>
                            {formated}
                        </div>
                    </>
                )
            }
        }, {
            id: "actions",
            header: "Ações",
            cell: ({ getValue, row }) => {
                const user = row.original;
                return (
                    <>
                        <DropdownMenu >
                            <DropdownMenuTrigger asChild>
                                <div className="flex items-center justify-center gap-2 flex-row-reverse">
                                    <Button variant="ghost" size="icon" className="rounded-full">
                                        <MoreHorizontal className="h-4 w-4"/>
                                    </Button>
                                </div>
                            </DropdownMenuTrigger>
                            <DropdownMenuContent align="end" className="w-[200px]">
                                <DropdownMenuGroup>
                                    <DropdownMenuItem
                                        onSelect={(e) => e.preventDefault()}
                                    >
                                        <DialogAccounts socialAccounts={user.socialAccounts}/>
                                    </DropdownMenuItem>
                                    <DropdownMenuSub>
                                        <DropdownMenuSubTrigger>Modificar função</DropdownMenuSubTrigger>
                                        <DropdownMenuPortal>
                                            <DropdownMenuSubContent>
                                                <DropdownMenuItem>Administrador</DropdownMenuItem>
                                                <DropdownMenuItem>Client</DropdownMenuItem>
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
        }
    ]
    return columns;
}