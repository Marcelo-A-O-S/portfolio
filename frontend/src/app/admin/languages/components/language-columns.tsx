import { LanguageSchema } from "@/domain/schemas/LanguageSchema"
import { ColumnDef } from "@tanstack/react-table"
import { format } from "date-fns"
import { ptBR } from "date-fns/locale"
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
            id:"actions",
            header: "Ações"
            
        }
    ]
    return columns;
}