"use client"
import { User } from "@/domain/types/User"
import { ColumnDef } from "@tanstack/react-table"
import { Button } from "@/components/ui/button"
import { DropdownMenuTrigger, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuSeparator, DropdownMenu, DropdownMenuSub, DropdownMenuSubTrigger, DropdownMenuPortal, DropdownMenuSubContent } from "@/components/ui/dropdown-menu"
import { LogOutIcon, MoreHorizontal } from "lucide-react"
import Link from "next/link"
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";
import DialogAccounts from "./dialog-accounts"
import { modifyRoleService } from "@/services/client/user-services"
import { useModifyRole } from "@/hooks/useModifyRole"
import { UserActions } from "@/components/user-actions"
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
                const formated = format(new Date(createdAt), "dd/MM/yyyy", { locale: ptBR });
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
            cell: ({ row })  => <UserActions user={row.original} />
        }
    ]
    return columns;
}