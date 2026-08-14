import { LinkTypeSchema } from "@/domain/schemas/LinkTypeSchema"
import { ColumnDef } from "@tanstack/react-table"

export const getLinkTypesColumns = () => {
    const columns: ColumnDef<LinkTypeSchema>[] = [
        {
            accessorKey: "name",
            header: "Nome"
        },
        {
            accessorKey: "backgroundColor",
            header: "Cor de fundo"
        },
        {
            accessorKey: "textColor",
            header: "Cor do texto"
        },
        {
            accessorKey: "icon",
            header: "Icone"
        }
    ]
    return columns;
}