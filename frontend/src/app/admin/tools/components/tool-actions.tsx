import { Button } from "@/components/ui/button"
import { DropdownMenu, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuSeparator, DropdownMenuTrigger } from "@/components/ui/dropdown-menu"
import { ToolSchema } from "@/domain/schemas/ToolSchema"
import { useDeleteTool } from "@/hooks/useDeleteTool"
import { MoreHorizontal } from "lucide-react"
import Link from "next/link"

type ToolActionsProps = {
    tool: ToolSchema
}
export default function ToolActions({ tool }: ToolActionsProps) {
    const { mutate } = useDeleteTool();
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
                        <DropdownMenuItem className="cursor-pointer" 
                        >
                            <Link href={`/admin/tools/manager?toolId=${tool.id}`}>Atualizar Ferramenta</Link>
                        </DropdownMenuItem>
                    </DropdownMenuGroup>
                    <DropdownMenuSeparator />
                    <DropdownMenuItem className="cursor-pointer" 
                         onClick={() =>
                        tool.id ? mutate(tool.id) : console.log("Identificador não informado")}
                    >
                        Deletar Ferramenta
                    </DropdownMenuItem>
                </DropdownMenuContent>
            </DropdownMenu>
        </>
    )
}