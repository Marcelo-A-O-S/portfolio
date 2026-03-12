"use client"
import { User } from "@/domain/types/User"
import { ColumnDef } from "@tanstack/react-table"
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";
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