import { CertificateSchema } from "@/domain/schemas/CertificateSchema"
import { ColumnDef } from "@tanstack/react-table"
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";
import { ImageIcon } from "lucide-react";
import { CertificateActions } from "./certificate-actions";

export const getCertificateColumns = () => {
    const columns: ColumnDef<CertificateSchema>[] = [
        {
            header: "Certificate",
            cell: ({ row }) => {
                const certificate = row.original;
                const certificatePreview = `${process.env.NEXT_PUBLIC_FILES_URL}/${certificate.media?.url}`;
                return (
                    <>
                        {certificate.media?.url ? (
                            <div className="flex flex-col justify-center items-center">
                                <div className="relative">
                                    <img
                                        src={certificatePreview}
                                        alt="Preview"
                                        className="h-32 rounded border object-cover"
                                    />
                                </div>
                            </div>
                        ) : (
                            <>
                                <div className="flex flex-col justify-center items-center h-32">
                                    <ImageIcon />
                                    Imagem não adicionada
                                </div >
                            </>
                        )}
                    </>
                )
            }
        },
        {
            header: "Content",
            cell: ({ row }) => {
                const certificate = row.original;
                return (
                    <>
                        <div>
                            <p>{certificate.title}</p>
                            <p>{certificate.description}</p>
                        </div>
                    </>
                )
            }
        },
        {
            header: "Institution",
            cell: ({ row }) => {
                const certificate = row.original;
                return (
                    <>
                        <div>
                            <p>{certificate.institution}</p>
                        </div>
                    </>
                )
            }
        },
        {
            header: "Data Emissão",
            cell: ({ row }) => {
                const certificate = row.original;
                console.log("Data de emissão: ", certificate.issuerDate)
                const issuerDate = format(new Date(certificate.issuerDate), "dd/MM/yyyy", { locale: ptBR });
                return (
                    <>
                        <div>
                            <p>{issuerDate}</p>
                        </div>
                    </>
                )
            }
        },
        {
            header: "Actions",
            cell: ({ row }) => {
                const certificate = row.original;
                return (
                    <div className="flex flex-col justify-center items-center">
                        <CertificateActions certificate={certificate} />
                    </div>
                )
            }
        }
    ]
    return columns;
}