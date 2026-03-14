"use client"
import DialogAccounts from "@/app/admin/users/components/dialog-accounts"
import { useModifyRole } from "@/hooks/useModifyRole"
import { MoreHorizontal } from "lucide-react"
import Link from "next/link"
import { Button } from "../../../../components/ui/button"
import { DropdownMenu, DropdownMenuTrigger, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuSub, DropdownMenuSubTrigger, DropdownMenuPortal, DropdownMenuSubContent, DropdownMenuSeparator } from "../../../../components/ui/dropdown-menu"
import { CategorySchema } from "@/domain/schemas/CategorySchema"
interface CategoryActionsProps {
    category: CategorySchema
}
export function CategoryActions({ category }: CategoryActionsProps) {
    
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
                        
                        
                        <DropdownMenuItem>
                            <Link className="text-sm w-full" href={"/admin/categories"}>Atualizar categoria</Link>
                        </DropdownMenuItem>
                    </DropdownMenuGroup>
                    <DropdownMenuSeparator />
                    <DropdownMenuItem className="cursor-pointer" >
                        Deletar categoria
                    </DropdownMenuItem>
                </DropdownMenuContent>
            </DropdownMenu>
        </>
    )
}