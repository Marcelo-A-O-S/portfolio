import { Button } from "@/components/ui/button"
import { DropdownMenu, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuSeparator, DropdownMenuTrigger } from "@/components/ui/dropdown-menu"
import { LinkTypeSchema } from "@/domain/schemas/LinkTypeSchema"
import { MoreHorizontal } from "lucide-react"
import FormLinkType from "./form-link-type"
import { useDeleteLinkType } from "@/hooks/LinkType/useDeleteLinkType"

type LinkTypeActionsProps = {
    linkType: LinkTypeSchema
}
export function LinkTypeActions({ linkType }: LinkTypeActionsProps) {
    const {mutate} = useDeleteLinkType();
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
                        {linkType.id ? (
                            <FormLinkType linkType={linkType} />
                        ) : (
                            <></>
                        )}
                    </DropdownMenuGroup>
                    <DropdownMenuSeparator />
                    <DropdownMenuItem className="cursor-pointer" onClick={() =>
                        linkType.id ? mutate({id: linkType.id, data: linkType}) : console.log("Identificador não informado")
                    }>
                        Delete link type
                    </DropdownMenuItem>
                </DropdownMenuContent>
            </DropdownMenu>
        </>
    )
}