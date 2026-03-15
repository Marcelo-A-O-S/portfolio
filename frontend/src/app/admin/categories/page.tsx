"use client";
import { Input } from "@/components/ui/input";
import { useSession } from "next-auth/react";
import { useRouter, useSearchParams } from "next/navigation";
import { useEffect, useMemo, useState } from "react";
import FormCategory from "./components/form-category";
import { useCategories } from "@/hooks/useCategories";
import { getCategoryColumns } from "./components/category-columns";
import { DataTable } from "@/components/data-table";
import { Skeleton } from "@/components/ui/skeleton";
import { useDebounce } from "@/hooks/useDebounce";
import { createPageURL, generatePagination } from "@/lib/utils";
import { toast } from "sonner";
import { Pagination, PaginationContent, PaginationItem, PaginationPrevious, PaginationEllipsis, PaginationLink, PaginationNext } from "@/components/ui/pagination";
export default function ToolsPage() {
    const { data: session } = useSession();
    const router = useRouter();
    const searchParams = useSearchParams();
    const page = Number(searchParams.get("page")) || 1;
    const language = searchParams.get("language") || undefined;
    const search = searchParams.get("search") || undefined;
    const [searchInput, setSearchInput] = useState(search ?? "");
    const debouncedSearch = useDebounce(searchInput, 500);
    const columns = useMemo(() => getCategoryColumns(), []);
    const { data, isLoading, error } = useCategories({
        page,
        language,
        search,
    })
    useEffect(() => {
        const params = new URLSearchParams(searchParams)
        params.set("page", "1")
        if (language) {
            params.set("language", language)
        }
        if (debouncedSearch) {
            params.set("search", debouncedSearch)
        } else {
            params.delete("search")
        }
        router.push(`?${params.toString()}`)
    }, [debouncedSearch])
 
    const totalPages = data?.totalPages || 1;
    const currentPage = data?.currentPage || 1;
    const pages = generatePagination(currentPage, totalPages);
    if (error) {
        toast.error("Erro ao buscar usuários")
    }
    return (
        <main className="relative mx-auto flex min-h-screen inset-0 w-screen max-w-[1440px] justify-center bg-white dark:bg-black ">
            <section className="relative w-screen h-svh px-10 py-18">
                <div className="flex flex-col gap-3 sm:flex-row  py-10 md:p-10 sm:items-center justify-between">
                    <h1 className="text-3xl md:text-5xl font-semibold">Categorias</h1>
                    <div className="flex gap-2">
                        <FormCategory />
                        <Input
                            placeholder="Buscar categoria..."
                            value={searchInput}
                            onChange={(e) => {
                                setSearchInput(e.target.value);
                            }}
                        />
                    </div>
                </div>
                <div className="flex md:px-10 gap-2">

                </div>
                <div className="flex py-10 md:p-10">
                    {isLoading ? (
                        <Skeleton className="h-[400px] w-full" />
                    ) : (
                        <DataTable columns={columns} data={data.items ?? []} />

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
    )
}