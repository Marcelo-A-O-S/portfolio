import { Button } from "@/components/ui/button";
import { DropdownMenu, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuSeparator, DropdownMenuTrigger } from "@/components/ui/dropdown-menu";
import { LinkSchema } from "@/domain/schemas/LinkSchema"
import { useDeleteLinkTool } from "@/hooks/Tool/Link/useDeleteLinkTool"
import { MoreHorizontal } from "lucide-react";
import FormLinkTool from "./form-link-tool";

type LinkToolActionsProps = {
    link: LinkSchema
}
export function LinkToolActions({ link }: LinkToolActionsProps) {
    const { mutate } = useDeleteLinkTool();
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
                            <FormLinkTool link={link} toolId={link.toolId!} />
                        ) : (
                            <></>
                        )}
                    </DropdownMenuGroup>
                    <DropdownMenuSeparator />
                    <DropdownMenuItem className="cursor-pointer" onClick={() =>
                        link.id ? mutate({ id: link.id, data: { ...link, postId: undefined } }) : console.log("Identificador não informado")
                    }>
                        Delete link
                    </DropdownMenuItem>
                </DropdownMenuContent>
            </DropdownMenu>
        </>
    )
}