import { Button, buttonVariants } from "@/components/ui/button";
import { LinkTypeSchema } from "@/domain/schemas/LinkTypeSchema"
import { ColumnDef } from "@tanstack/react-table"
import Image from "next/image";
import { LinkTypeActions } from "./links-types-actions";
import Link from "next/link";

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
            accessorKey: "borderColor",
            header: "Borda"
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
                    <Link
                        href={""}
                        style={{
                            backgroundColor: linkType.backgroundColor,
                            color: linkType.textColor,
                            borderColor: linkType.borderColor,
                            border: "1px",
                            borderStyle: "solid"
                        }}
                        className={`${buttonVariants({ variant: "default" })} cursor-pointer`}
                    >
                        <Image src={linkType.icon} alt={linkType.name} width={20} height={20} />
                        {linkType.name}
                    </Link>
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