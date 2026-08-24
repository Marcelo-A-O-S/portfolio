"use client"
import { useDebounce } from "@/hooks/useDebounce";
import { useSession } from "next-auth/react";
import { notFound, useRouter, useSearchParams } from "next/navigation";
import { useEffect, useMemo, useState } from "react";
import { getContributorsColumns } from "./components/contributor-columns";
import { usePaginationContributor } from "@/hooks/Contributor/usePaginationContributor";
import { Skeleton } from "@/components/ui/skeleton";
import { DataTable } from "@/components/data-table";
import { createPageURL, generatePagination } from "@/lib/utils";
import { Input } from "@/components/ui/input";
import { Pagination, PaginationContent, PaginationEllipsis, PaginationItem, PaginationLink, PaginationNext, PaginationPrevious } from "@/components/ui/pagination";
import FormContributorPost from "./components/form-contributor-post";

export default function ContributorsPage() {
    const { data: session } = useSession();
    const router = useRouter();
    const searchParams = useSearchParams();
    const page = Number(searchParams.get("page")) || 1;
    const postId = searchParams.get("postId") || undefined;
    const search = searchParams.get("search") || undefined;
    const [searchInput, setSearchInput] = useState(search ?? "");
    const debouncedSearch = useDebounce(searchInput, 500);
    const columns = useMemo(() => getContributorsColumns(), []);
    if (postId == undefined)
        notFound();
    const { data, isLoading, error } = usePaginationContributor({
        page,
        postId,
        search
    })
    const totalPages = data?.totalPages || 1;
    const currentPage = data?.currentPage || 1;
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
    console.log("Contribuidores: ", data);
    return (
        <>
            <main className="mx-auto flex min-h-screen inset-0 w-full justify-center">
                <section className="relative w-full h-svh px-10 py-18">
                    <div className="flex flex-col gap-3 sm:flex-row  py-10 md:p-10 sm:items-center justify-between">
                        <h1 className="text-3xl md:text-5xl font-semibold">Contribuidores</h1>
                        <div className="flex gap-2">
                            <FormContributorPost
                                postId={postId}
                            />
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
                            <DataTable columns={columns} data={data?.items ?? []} />
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