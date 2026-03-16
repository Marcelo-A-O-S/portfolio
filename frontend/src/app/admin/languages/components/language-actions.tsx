import { LanguageSchema } from "@/domain/schemas/LanguageSchema"
import { useDeleteLanguage } from "@/hooks/useDeleteLanguage";
import FormLanguage from "./form-language";
import { Button } from "@/components/ui/button";
import { DropdownMenuTrigger, DropdownMenuContent, DropdownMenuGroup, DropdownMenuSeparator, DropdownMenuItem, DropdownMenu } from "@/components/ui/dropdown-menu";
import { MoreHorizontal } from "lucide-react";
type LanguageActionsProps = {
    language: LanguageSchema
}
export default function LanguageActions({ language }: LanguageActionsProps) {
    const { mutate } = useDeleteLanguage();
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
                        {language.id ? (
                            <FormLanguage language={language} />
                        ) : (
                            <></>
                        )}
                    </DropdownMenuGroup>
                    <DropdownMenuSeparator />
                    <DropdownMenuItem className="cursor-pointer" onClick={() =>
                        language.id ? mutate(language.id) : console.log("Identificador não informado")
                    }>
                        Deletar linguagem
                    </DropdownMenuItem>
                </DropdownMenuContent>
            </DropdownMenu>
        </>
    )
}