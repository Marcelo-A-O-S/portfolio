import { buttonVariants } from "@/components/ui/button";
import { LinkSchema } from "@/domain/schemas/LinkSchema"
import { ColumnDef } from "@tanstack/react-table"
import Image from "next/image";
import Link from "next/link";
import { LinkToolActions } from "./link-tool-actions";

export const getLinksColumns = () => {
    const columns: ColumnDef<LinkSchema>[] = [
        {
            accessorKey: "title",
            header: "Title",
        },
        {
            accessorKey: "url",
            header: "Url"
        },
        {
            header: "Preview",
            cell: ({ row }) => {
                const link = row.original;
                return (
                    <Link
                        href={""}
                        style={{
                            backgroundColor: link.linkType?.backgroundColor,
                            color: link.linkType?.textColor,
                            borderColor: link.linkType?.borderColor,
                            border: "1px",
                            borderStyle: "solid"
                        }}
                        className={`${buttonVariants({ variant: "default" })} cursor-pointer`}
                    >
                        <Image src={link.linkType!.icon} alt={link.linkType!.name} width={20} height={20} />
                        {link.linkType?.name} | {link.title}
                    </Link>
                )
            }
        },
        {
            id: "actions",
            header: "Ações",
            cell: ({ row }) => <LinkToolActions link={row.original}  />
        }
    ]
    return columns;
}