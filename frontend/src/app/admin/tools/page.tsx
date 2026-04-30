"use client";
import { buttonVariants } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { useLanguages } from "@/hooks/useLanguages";
import { usePaginationTool } from "@/hooks/usePaginationTool";
import { useSession } from "next-auth/react";
import Link from "next/link";
import { useRouter, useSearchParams } from "next/navigation";
import { useState } from "react";
import CardTool from "./components/card-tool";
import { Pagination, PaginationContent, PaginationItem, PaginationPrevious, PaginationEllipsis, PaginationLink, PaginationNext } from "@/components/ui/pagination";
import { createPageURL, generatePagination } from "@/lib/utils";
export default function ToolsPage() {
    const { data: session } = useSession();
    const router = useRouter();
    const searchParams = useSearchParams();
    const page = Number(searchParams.get("page")) || 1;
    const search = searchParams.get("search") || undefined;
    const [searchInput, setSearchInput] = useState(search ?? "");
    const { data: languages } = useLanguages();
    const { data: tools } = usePaginationTool({
        page,
        search
    });
    const totalPages = tools?.totalPages || 1;
    const currentPage = tools?.currentPage || 1;
    const pages = generatePagination(currentPage, totalPages);
    return (
        <main className="relative mx-auto flex min-h-screen inset-0 w-full max-w-[1440px] justify-center  ">
            <section className="relative w-full min-h-screen px-10 py-20 flex flex-col">
                <div className="flex flex-col gap-3 sm:flex-row  py-10 md:p-10 sm:items-center justify-between">
                    <h1 className="text-3xl md:text-5xl font-semibold">Ferramentas</h1>
                    <div className="flex gap-2">
                        <Link className={buttonVariants({ variant: "default" }) + ``} href={"/admin/tools/manager"}>Add Tool</Link>
                        <Input
                            placeholder="Buscar ferramenta..."
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
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 ">
                        {tools?.items.map((item, index) => (
                            <CardTool key={index} item={item} languages={languages} />
                        ))}
                    </div>
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
        </main >
    )
}