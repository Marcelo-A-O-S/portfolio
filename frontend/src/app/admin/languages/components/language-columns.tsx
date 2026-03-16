import { LanguageSchema } from "@/domain/schemas/LanguageSchema"
import { ColumnDef } from "@tanstack/react-table"
import { format } from "date-fns"
import { ptBR } from "date-fns/locale"
import LanguageActions from "./language-actions"
export const getLanguageColumns = () => {
    const columns: ColumnDef<LanguageSchema>[] = [
        {
            accessorKey: "name",
            header: "Nome"
        },
        {
            accessorKey: "code",
            header: "Código"
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
        },
        {
            accessorKey: "updatedAt",
            header: "Ultima atualização",
            cell: ({ getValue }) => {
                const updatedAt = getValue() as string
                const formated = format(new Date(updatedAt), "dd/MM/yyyy", { locale: ptBR });
                return (
                    <>
                        <div>
                            {formated}
                        </div>
                    </>
                )
            }

        },
        {
            id: "actions",
            header: "Ações",
            cell: ({row}) => <LanguageActions language={row.original} />
        }
    ]
    return columns;
}