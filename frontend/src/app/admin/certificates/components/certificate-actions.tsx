import { Button } from "@/components/ui/button";
import { DropdownMenu, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuSeparator, DropdownMenuTrigger } from "@/components/ui/dropdown-menu";
import { CertificateSchema } from "@/domain/schemas/CertificateSchema"
import { useDeleteCertificate } from "@/hooks/Certificate/useDeleteCertificate"
import { MoreHorizontal } from "lucide-react";
import Link from "next/link";

type CertificateActionsProps = {
    certificate: CertificateSchema
}
export function CertificateActions({ certificate }: CertificateActionsProps) {
    const { mutate } = useDeleteCertificate();
    return (
        <>
            <DropdownMenu >
                <DropdownMenuTrigger asChild>
                    <div className="flex items-center justify-center gap-2 min-w-0 flex-1 flex-row-reverse">
                        <Button variant="ghost" size="icon" className="rounded-full">
                            <MoreHorizontal className="h-4 w-4" />
                        </Button>
                    </div>
                </DropdownMenuTrigger>
                <DropdownMenuContent align="end" className="w-[200px]">
                    <DropdownMenuGroup>
                        <DropdownMenuItem className="cursor-pointer"
                        >
                            <Link href={`/admin/certificates/manager-projects?toolId=${certificate.id}`}>Gerenciar Projetos</Link>
                        </DropdownMenuItem>
                        <DropdownMenuItem className="cursor-pointer"
                        >
                            <Link href={`/admin/certificates/manager?certificateId=${certificate.id}`}>Atualizar Certificado</Link>
                        </DropdownMenuItem>
                    </DropdownMenuGroup>
                    <DropdownMenuSeparator />
                    <DropdownMenuItem className="cursor-pointer"
                        onClick={() =>
                            certificate.id ? mutate({id: certificate.id, data: certificate}) : console.log("Identificador não informado")}
                    >
                        Deletar Certificado
                    </DropdownMenuItem>
                </DropdownMenuContent>
            </DropdownMenu>
        </>
    )
}