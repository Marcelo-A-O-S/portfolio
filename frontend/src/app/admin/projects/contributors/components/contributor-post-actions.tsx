import { Button } from "@/components/ui/button";
import { DropdownMenu, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuSeparator, DropdownMenuTrigger } from "@/components/ui/dropdown-menu";
import { ContributorSchema } from "@/domain/schemas/ContributorSchema"
import { useDeleteContributorPost } from "@/hooks/Post/Contributor/useDeleteContributorPost";
import { MoreHorizontal } from "lucide-react";
import FormContributorPost from "./form-contributor-post";

type ContributorPostActionsProps = {
    contributor: ContributorSchema
}
export function ContributorPostActions({contributor}: ContributorPostActionsProps){
    const { mutate } = useDeleteContributorPost();
    return(
        <>
            <DropdownMenu >
                <DropdownMenuTrigger asChild>
                    <div className="flex items-center justify-center gap-2 flex-row-reverse">
                        <Button variant="ghost" size="icon" className="rounded-full">
                            <MoreHorizontal className="h-4 w-4" />
                        </Button>
                    </div>
                </DropdownMenuTrigger>
                <DropdownMenuContent align="end" className="w-[200px]">
                    <DropdownMenuGroup>
                        {contributor.id ? (
                            <FormContributorPost contributor={contributor} postId={contributor.postId} />
                        ) : (
                            <></>
                        )}
                    </DropdownMenuGroup>
                    <DropdownMenuSeparator />
                    <DropdownMenuItem className="cursor-pointer" onClick={() =>
                        contributor.id ? mutate({ id: contributor.id, data: contributor }) : console.log("Identificador não informado")
                    }>
                        Delete Contributor
                    </DropdownMenuItem>
                </DropdownMenuContent>
            </DropdownMenu>
        </>
    )
}