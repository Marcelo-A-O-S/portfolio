import { LinkSchema } from "@/domain/schemas/LinkSchema"
import { ColumnDef } from "@tanstack/react-table"

export const getLinksColumns = () =>{
    const columns: ColumnDef<LinkSchema>[]=[
        {
            accessorKey: "title",
            header: "Title",
        },
        {
            accessorKey: "url",
            header: "Url"
        }
    ]
    return columns;
}