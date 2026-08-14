"use client";
import { Input } from "@/components/ui/input";
import FormLinkType from "./components/form-link-type";
import { useSession } from "next-auth/react";
import { useRouter, useSearchParams } from "next/navigation";
import { useEffect, useMemo, useState } from "react";
import { useDebounce } from "@/hooks/useDebounce";
import { usePaginationLinkType } from "@/hooks/LinkType/usePaginationLinkType";
import { getLinkTypesColumns } from "./components/links-types-columns";
import { Skeleton } from "@/components/ui/skeleton";
import { DataTable } from "@/components/data-table";
import { createPageURL, generatePagination } from "@/lib/utils";
import { Pagination, PaginationContent, PaginationEllipsis, PaginationItem, PaginationLink, PaginationNext, PaginationPrevious } from "@/components/ui/pagination";
export default function LinksTypesPage() {
    const { data: session } = useSession();
    const router = useRouter();
    const searchParams = useSearchParams();
    const page = Number(searchParams.get("page")) || 1;
    const search = searchParams.get("search") || undefined;
    const [searchInput, setSearchInput] = useState(search ?? "");
    const debouncedSearch = useDebounce(searchInput, 500);
    const columns = useMemo(() => getLinkTypesColumns(), [])
    const { data: linkTypes, isLoading, error } = usePaginationLinkType({
        page,
        search
    })
    const totalPages = linkTypes?.totalPages || 1;
    const currentPage = linkTypes?.currentPage || 1;
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
    return (
        <>
            <main className="relative mx-auto flex min-h-screen inset-0 w-full justify-center ">
                <section className="relative w-screen h-svh px-10 py-18">
                    <div className="flex flex-col gap-3 sm:flex-row  py-10 md:p-10 sm:items-center justify-between">
                        <h1 className="text-3xl md:text-5xl font-semibold">Links Types</h1>
                        <div className="flex gap-2">
                            <FormLinkType />
                            <Input
                                placeholder={"Search type..."}
                                value={searchInput}
                                onChange={(e) => {
                                    setSearchInput(e.target.value);
                                }}
                            />
                        </div>
                    </div>
                    <div className="flex py-10 md:p-10">
                        {isLoading ? (
                            <Skeleton className="h-[400px] w-full" />
                        ) : (
                            <DataTable columns={columns} data={linkTypes?.items ?? []} />
                        )}
                    </div>
                    <div className="relative bottom-0 ">
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