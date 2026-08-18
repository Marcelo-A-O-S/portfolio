import LinkSection from "@/components/link-section";
import { LinkSchema } from "@/domain/schemas/LinkSchema";
import { ColumnDef } from "@tanstack/react-table";
import { LinkPostActions } from "./link-post-actions";

export const getLinksPostColumns = () => {
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
            cell: ({ row }) => <LinkPostActions link={row.original} />
        }
    ]
    return columns;
}