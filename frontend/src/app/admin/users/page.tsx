"use client";
import { DataTable } from "../../../components/data-table";
import { useRouter, useSearchParams } from "next/navigation";
import { useEffect, useMemo, useState } from "react";
import { toast } from "sonner";
import { getUsersColumns } from "./components/user-columns";
import { useSession } from "next-auth/react";
import { Pagination, PaginationContent, PaginationEllipsis, PaginationItem, PaginationLink, PaginationNext, PaginationPrevious } from "@/components/ui/pagination";
import { Skeleton } from "@/components/ui/skeleton";
import { Input } from "@/components/ui/input";
import { useUsers } from "@/hooks/useUsers";
import { createPageURL, generatePagination } from "@/lib/utils";
import { useDebounce } from "@/hooks/useDebounce";
import { SelectTrigger, SelectValue, SelectContent, SelectGroup, SelectLabel, SelectItem, Select } from "@/components/ui/select";
export default function UsersPage() {
    const { data: session } = useSession()
    const router = useRouter();
    const searchParams = useSearchParams();
    const page = Number(searchParams.get("page")) || 1;
    const search = searchParams.get("search") || undefined;
    const role = searchParams.get("role") || undefined;
    const status = searchParams.get("status") || undefined;
    const [searchInput, setSearchInput] = useState(search ?? "");
    const debouncedSearch = useDebounce(searchInput, 500);
    const columns = useMemo(() => getUsersColumns(), []);
    const { data, isLoading, error } = useUsers({
        page,
        search,
        role,
        status
    });
    useEffect(() => {
        const params = new URLSearchParams(searchParams)
        if (debouncedSearch) {
            params.set("search", debouncedSearch)
        } else {
            params.delete("search")
        }
        params.set("page", "1")
        router.push(`?${params.toString()}`)
    }, [debouncedSearch])
    const totalPages = data?.totalPages || 1;
    const currentPage = data?.currentPage || 1;
    const pages = generatePagination(currentPage, totalPages);
    if (error) {
        toast.error("Erro ao buscar usuários")
    }
   
    function updateFilter(key: string, value?: string) {
        const params = new URLSearchParams(searchParams)
        params.set("page", "1")
        if (!value || value === "ALL") {
            params.delete(key)
        } else {
            params.set(key, value)
        }
        router.push(`?${params.toString()}`)
    }
    return (
        <main className="relative mx-auto flex min-h-screen inset-0 w-screen max-w-[1440px] justify-center bg-white dark:bg-black ">
            <section className="relative w-full min-h-screen px-10 py-20 flex flex-col">
                <div className="flex py-10 md:p-10 items-center justify-between ">
                    <h1 className="text-3xl md:text-5xl font-semibold">Usuários</h1>
                    <div>
                        <Input
                            placeholder="Buscar usuário..."
                            value={searchInput}
                            onChange={(e) => {
                                setSearchInput(e.target.value);
                            }}
                        />
                    </div>
                </div>
                <div className="flex md:px-10 gap-2">
                    <Select
                        value={role}
                        onValueChange={(value) => updateFilter("role", value)}
                    >
                        <SelectTrigger className="w-full max-w-48">
                            <SelectValue
                                placeholder="Selecionar função" />
                        </SelectTrigger>
                        <SelectContent>
                            <SelectGroup>
                                <SelectLabel>Funções</SelectLabel>
                                <SelectItem value="Client">Client</SelectItem>
                                <SelectItem value="Administrador">Administrador</SelectItem>
                            </SelectGroup>
                        </SelectContent>
                    </Select>
                    <Select
                        value={status}
                        onValueChange={(value) => updateFilter("status", value)}
                    >
                        <SelectTrigger className="w-full max-w-48">
                            <SelectValue
                                placeholder="Selecionar status" />
                        </SelectTrigger>
                        <SelectContent>
                            <SelectGroup>
                                <SelectLabel>Status</SelectLabel>
                                <SelectItem value="ACTIVE">Active</SelectItem>
                                <SelectItem value="SUSPEND">Suspend</SelectItem>
                                <SelectItem value="BANNED">Banned</SelectItem>
                            </SelectGroup>
                        </SelectContent>
                    </Select>
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
                                <PaginationPrevious href={createPageURL(Math.max(currentPage - 1, 1),searchParams)} />
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
                                            href={createPageURL(page,searchParams)}
                                        >
                                            {page}
                                        </PaginationLink>
                                    </PaginationItem>
                                )
                            })}
                            <PaginationItem>
                                <PaginationNext
                                    href={createPageURL(Math.min(currentPage + 1, totalPages),searchParams)}
                                />
                            </PaginationItem>
                        </PaginationContent>
                    </Pagination>
                </div>
            </section>
        </main>
    )
}