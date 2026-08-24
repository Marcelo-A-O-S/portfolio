import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { ContributorSchema } from "@/domain/schemas/ContributorSchema"
import { ColumnDef } from "@tanstack/react-table"
import { ContributorPostActions } from "./contributor-post-actions";

export const getContributorsColumns = () => {
    const columns: ColumnDef<ContributorSchema>[] = [
        {
            header: "Preview",
            cell: ({ row }) => {
                const link = row.original;
                return (
                    <div className="flex gap-2 items-center">
                        <div className="rounded-full">
                            <Avatar>
                                <AvatarImage src={link.profileUrl!} alt={link.name!} />
                                <AvatarFallback>LR</AvatarFallback>
                            </Avatar>
                        </div>
                        <div>
                            <p className="hidden md:flex text-sm truncate w-full font-semibold">{link.name}</p>
                            <p>{link.description}</p>
                        </div>
                    </div>
                )
            }
        },
        {
            header:"Ações",
            cell:({row})=>{
                const contributor = row.original;
                return <ContributorPostActions contributor={contributor} />
            }
        }
    ]
    return columns;
}