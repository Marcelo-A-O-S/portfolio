import { Button } from "@/components/ui/button";
import { DropdownMenuTrigger, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuSeparator, DropdownMenu } from "@/components/ui/dropdown-menu";
import { PostSchema } from "@/domain/schemas/PostSchema"
import { useDeleteProject } from "@/hooks/Post/useDeleteProject"
import { MoreHorizontal } from "lucide-react";
import Link from "next/link";


type ProjectActionsProps = {
    project: PostSchema
}
export default function ProjectActions({ project }: ProjectActionsProps) {
    const { mutate } = useDeleteProject();
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
                            <Link href={`/admin/projects/manager?postId=${project.id}`}>Atualizar Projeto</Link>
                        </DropdownMenuItem>
                    </DropdownMenuGroup>
                    <DropdownMenuSeparator />
                    <DropdownMenuItem className="cursor-pointer"
                        onClick={() =>
                            project.id ? mutate(project.id) : console.log("Identificador não informado")}
                    >
                        Deletar Projeto
                    </DropdownMenuItem>
                </DropdownMenuContent>
            </DropdownMenu>
        </>
    )
}