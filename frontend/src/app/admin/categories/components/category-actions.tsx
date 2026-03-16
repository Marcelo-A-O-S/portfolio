"use client"
import { MoreHorizontal } from "lucide-react"
import Link from "next/link"
import { Button } from "../../../../components/ui/button"
import { DropdownMenu, DropdownMenuTrigger, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuSeparator } from "../../../../components/ui/dropdown-menu"
import { CategorySchema } from "@/domain/schemas/CategorySchema"
import { useDeleteCategory } from "@/hooks/useDeleteCategory"
import FormCategory from "./form-category"
interface CategoryActionsProps {
    category: CategorySchema
}
export function CategoryActions({ category }: CategoryActionsProps) {
    const { mutate } = useDeleteCategory();
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
                        {category.id ? (
                            <FormCategory category={category} />
                        ) : (
                            <></>
                        )}
                    </DropdownMenuGroup>
                    <DropdownMenuSeparator />
                    <DropdownMenuItem className="cursor-pointer" onClick={() =>
                        category.id ? mutate(category.id) : console.log("Identificador não informado")
                    }>
                        Deletar categoria
                    </DropdownMenuItem>
                </DropdownMenuContent>
            </DropdownMenu>
        </>
    )
}