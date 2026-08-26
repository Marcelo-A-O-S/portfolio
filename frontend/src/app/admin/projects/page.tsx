"use client";
import { buttonVariants } from "@/components/ui/button";
import Link from "next/link";
import { useRouter, useSearchParams } from "next/navigation";
import { useEffect, useState } from "react";
import { Input } from "@/components/ui/input";
import { useLanguages } from "@/hooks/Language/useLanguages";
import { useSession } from "next-auth/react";
import { usePaginationProject } from "@/hooks/Post/usePaginationProject";
import CardProject from "./components/card-project";
import { createPageURL, generatePagination } from "@/lib/utils";
import { useDebounce } from "@/hooks/useDebounce";
import { Pagination, PaginationContent, PaginationEllipsis, PaginationItem, PaginationLink, PaginationNext, PaginationPrevious } from "@/components/ui/pagination";
import { Skeleton } from "@/components/ui/skeleton";
export default function ProjectPage() {
    const { data: session } = useSession();
    const router = useRouter();
    const searchParams = useSearchParams();
    const page = Number(searchParams.get("page")) || 1;
    const search = searchParams.get("search") || undefined;
    const [searchInput, setSearchInput] = useState(search ?? "");
    const debouncedSearch = useDebounce(searchInput, 500);
    const { data: languages } = useLanguages();
    const { data: projects, isLoading, error } = usePaginationProject({
        page,
        search
    })
    const totalPages = projects?.totalPages || 1;
    const currentPage = projects?.currentPage || 1;
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
            <main className="relative mx-auto flex min-h-screen inset-0 w-full justify-center">
                <section className="relative w-full min-h-screen px-10 py-18">
                    <div className="flex p-10 items-center justify-between">
                        <h1 className="text-3xl md:text-5xl font-semibold">Projects</h1>
                        <div className="flex gap-2 items-center">
                            <Link className={buttonVariants({ variant: "default" }) + ``} href={"/admin/projects/manager"}>Create Post</Link>
                            <Input
                                placeholder="Buscar postagem..."
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
                        {isLoading? (
                            <Skeleton className="h-[400px] w-full" />
                        ):(
                            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 items-stretch">
                                {projects?.items.map((item, index) => (
                                    <CardProject key={index} item={item} languages={languages} />
                                ))}
                            </div>
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