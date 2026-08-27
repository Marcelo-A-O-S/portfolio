"use client";
import { buttonVariants } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Pagination, PaginationContent, PaginationEllipsis, PaginationItem, PaginationLink, PaginationNext, PaginationPrevious } from "@/components/ui/pagination";
import { Skeleton } from "@/components/ui/skeleton";
import { usePaginationCertificate } from "@/hooks/Certificate/usePaginationCertificate";
import { useDebounce } from "@/hooks/useDebounce";
import { createPageURL, generatePagination } from "@/lib/utils";
import { useSession } from "next-auth/react";
import Link from "next/link";
import { useRouter, useSearchParams } from "next/navigation";
import { useEffect, useMemo, useState } from "react";
import { getCertificateColumns } from "./components/certificate-columns";
import { DataTable } from "@/components/data-table";

export default function CertificatePage() {
    const { data: session } = useSession();
    const router = useRouter();
    const searchParams = useSearchParams();
    const page = Number(searchParams.get("page")) || 1;
    const search = searchParams.get("search") || undefined;
    const [searchInput, setSearchInput] = useState(search ?? "");
    const debouncedSearch = useDebounce(searchInput, 500);
    const columns = useMemo(() => getCertificateColumns(), [])
    const { data: certificates, isLoading, error } = usePaginationCertificate({
        page,
        search
    })
    const totalPages = certificates?.totalPages || 1;
    const currentPage = certificates?.currentPage || 1;
    const pages = generatePagination(currentPage, totalPages);
    useEffect(() => {
        const params = new URLSearchParams(searchParams)
        if (debouncedSearch) {
            params.set("search", debouncedSearch)
        } else {
            params.delete("search")
        }
        router.push(`?${params.toString()}`)
    }, [debouncedSearch])
    console.log("Certificados: ", certificates)
    return (
        <>
            <main className="relative mx-auto flex min-h-screen inset-0 w-full justify-center">
                <section className="relative w-full min-h-screen px-10 py-18">
                    <div className="flex p-10 items-center justify-between">
                        <h1 className="text-3xl md:text-5xl font-semibold">Certificates</h1>
                        <div className="flex gap-2 items-center">
                            <Link className={buttonVariants({ variant: "default" }) + ``} href={"/admin/certificates/manager"}>Add Certificate</Link>
                            <Input
                                placeholder="Buscar certificado..."
                                value={searchInput}
                                onChange={(e) => {
                                    setSearchInput(e.target.value);
                                }}
                            />
                        </div>
                    </div>
                    <div className="flex md:px-10 gap-2">
                    </div>
                    <div className="flex justify-center py-10 md:p-10">
                        {isLoading?(
                            <Skeleton className="h-[400px] w-full" />
                        ):(
                            <DataTable columns={columns} data={certificates?.items ?? []} />
                        )}
                    </div>
                    <div className="relative bottom-0">
                        <Pagination>
                            <PaginationContent>
                                <PaginationItem>
                                    <PaginationPrevious href={createPageURL(Math.max(currentPage - 1, 1), searchParams)} />
                                </PaginationItem>
                                {pages.map((page, index) => {
                                    if (page === "ellipsis") {
                                        return (
                                            <PaginationItem key={`ellipsis-${index}`}>
                                                <PaginationEllipsis />
                                            </PaginationItem>
                                        )
                                    }
                                    return (
                                        <PaginationItem key={page}>
                                            <PaginationLink
                                                isActive={page === currentPage}
                                                href={createPageURL(page, searchParams)}
                                            >
                                                {page}
                                            </PaginationLink>
                                        </PaginationItem>
                                    )
                                })}
                                <PaginationItem>
                                    <PaginationNext
                                        href={createPageURL(Math.min(currentPage + 1, totalPages), searchParams)}
                                    />
                                </PaginationItem>
                            </PaginationContent>
                        </Pagination>
                    </div>
                </section>
            </main>
        </>
    )
}