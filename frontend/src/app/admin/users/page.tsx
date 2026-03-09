"use client";
import { buttonVariants } from "@/components/ui/button";
import Link from "next/link";
import { DataTable } from "./components/data-table";
import { useRouter, useSearchParams } from "next/navigation";
import { useEffect, useMemo, useState } from "react";
import { User } from "@/domain/types/User";
import { getUsersByPaginationService } from "@/services/client/user-services";
import { toast } from "sonner";
import { getUsersColumns } from "./components/user-columns";
import { useSession } from "next-auth/react";
import { useQuery } from "@tanstack/react-query";
import { Pagination, PaginationContent, PaginationEllipsis, PaginationItem, PaginationLink, PaginationNext, PaginationPrevious } from "@/components/ui/pagination";
import { Skeleton } from "@/components/ui/skeleton";
import { Input } from "@/components/ui/input";
import { useUsers } from "@/hooks/useUsers";
export default function UsersPage() {
    const { data: session } = useSession()
    const router = useRouter();
    const searchParams = useSearchParams();
    const page = Number(searchParams.get("page")) || 1;
    const search = searchParams.get("search") || "";
    const role = searchParams.get("role") || ""
    const status = searchParams.get("status") || ""
    const columns = useMemo(() => getUsersColumns(), []);
    const { data, isLoading, error } = useUsers(page, search);
    if (error) {
        toast.error("Erro ao buscar usuários")
    }
    return (
        <main className="relative mx-auto flex min-h-screen inset-0 w-screen max-w-[1440px] justify-center bg-white dark:bg-black ">
            <section className="relative w-full min-h-screen px-10 py-20 flex flex-col">
                <div className="flex p-10 items-center justify-between ">
                    <h1 className="text-3xl md:text-5xl font-semibold">Usuários</h1>
                    <div>
                        <Input
                            placeholder="Buscar usuário..."
                            defaultValue={search}
                            onChange={(e) => {
                                router.push(`?page=1&search=${e.target.value}`);
                            }}
                        />
                    </div>
                </div>
                <div className="p-10">
                    {isLoading ? (
                        <Skeleton className="h-[400px] w-full" />
                    ) : (
                        <DataTable columns={columns} data={data ?? []} />
                    )}
                </div>
                <div>
                    <Pagination>
                        <PaginationContent>
                            <PaginationItem>
                                <PaginationPrevious href="#" />
                            </PaginationItem>
                            <PaginationItem>
                                <PaginationLink href="#">1</PaginationLink>
                            </PaginationItem>
                            <PaginationItem>
                                <PaginationLink href="#" isActive>
                                    2
                                </PaginationLink>
                            </PaginationItem>
                            <PaginationItem>
                                <PaginationLink href="#">3</PaginationLink>
                            </PaginationItem>
                            <PaginationItem>
                                <PaginationEllipsis />
                            </PaginationItem>
                            <PaginationItem>
                                <PaginationNext href="#" />
                            </PaginationItem>
                        </PaginationContent>
                    </Pagination>
                </div>
            </section>
        </main>
    )
}