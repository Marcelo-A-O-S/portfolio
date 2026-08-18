import { LinkSchema } from "@/domain/schemas/LinkSchema"
import { ColumnDef } from "@tanstack/react-table"
import { LinkToolActions } from "./link-tool-actions";
import LinkSection from "@/components/link-section";

export const getLinksToolColumns = () => {
    const columns: ColumnDef<LinkSchema>[] = [
        {
            accessorKey: "url",
            header: "Url"
        },
        {
            header: "Preview",
            cell: ({ row }) => {
                const link = row.original;
                return (
                    <LinkSection link={link}/>
                )
            }
        },
        {
            id: "actions",
            header: "Ações",
            cell: ({ row }) => <LinkToolActions link={row.original} />
        }
    ]
    return columns;
}