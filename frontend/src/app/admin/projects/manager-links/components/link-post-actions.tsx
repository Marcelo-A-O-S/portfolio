import { Button } from "@/components/ui/button"
import { DropdownMenu, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuSeparator, DropdownMenuTrigger } from "@/components/ui/dropdown-menu"
import { LinkSchema } from "@/domain/schemas/LinkSchema"
import { MoreHorizontal } from "lucide-react"
import FormLinkPost from "./form-link-post"
import { useDeleteLinkPost } from "@/hooks/Post/Link/useDeleteLinkPost"

type LinkPostActionsProps = {
    link: LinkSchema
}
export function LinkPostActions({ link }: LinkPostActionsProps) {
    const { mutate } = useDeleteLinkPost();
    return (
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
                        {link.id ? (
                            <FormLinkPost link={link} postId={link.postId!} />
                        ) : (
                            <></>
                        )}
                    </DropdownMenuGroup>
                    <DropdownMenuSeparator />
                    <DropdownMenuItem className="cursor-pointer" onClick={() =>
                        link.id ? mutate({ id: link.id, data: { ...link, toolId: undefined } }) : console.log("Identificador não informado")
                    }>
                        Delete link
                    </DropdownMenuItem>
                </DropdownMenuContent>
            </DropdownMenu>
        </>
    )
}