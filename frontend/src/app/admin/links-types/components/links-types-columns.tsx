import { Button } from "@/components/ui/button";
import { LinkTypeSchema } from "@/domain/schemas/LinkTypeSchema"
import { ColumnDef } from "@tanstack/react-table"
import Image from "next/image";
import { LinkTypeActions } from "./links-types-actions";

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
        },
        {
            header: "Preview",
            cell: ({ row }) => {
                const linkType = row.original;
                return (
                    <Button
                    style={{ 
                        backgroundColor: linkType.backgroundColor, 
                        color: linkType.textColor 
                    }} 
                    className="cursor-pointer"
                    type="button">
                        <Image src={linkType.icon} alt={linkType.name} width={20} height={20} />
                        {linkType.name}
                    </Button>
                )
            }
        },
        {
            id: "actions",
            header: "Ações",
            cell: ({ row }) => <LinkTypeActions linkType={row.original} />
        }
    ]
    return columns;
}