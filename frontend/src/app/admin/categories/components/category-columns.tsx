import { Category } from "@/domain/types/Category"
import { ColumnDef } from "@tanstack/react-table"
import { format } from "date-fns/format"
import { ptBR } from "date-fns/locale"
import { CategoryActions } from "./category-actions"
export const getCategoryColumns = () => {
    const columns: ColumnDef<Category>[] = [
        
        {
            id: "names",
            header: "Nomes",
            cell: ({ row }) => {
                const category = row.original;
                const names = category.categoryContents.map((c) => {
                    return c.name;
                })
                return (
                    <div className="flex flex-col">
                        {names.map((item, index) => (
                            <div key={index}>
                                {item}
                            </div>
                        ))}
                    </div>
                )
            }
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
            id: "languages",
            header: "Linguagens",
            cell: ({ row }) => {
                const category = row.original;
                const languages = category.categoryContents.map((c) => {
                    return c.language;
                })
                return (
                    <div className="flex flex-col">
                        {languages.map((item, index) => (
                            <div key={index}>
                                {item}
                            </div>
                        ))}
                    </div>
                )
            }
        },
        {
            id: "actions",
            header: "Ações",
            cell: ({ row }) => <CategoryActions category={row.original} />
        }
    ]
    return columns;
}