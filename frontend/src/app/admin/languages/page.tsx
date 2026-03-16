"use client";
import { useSession } from "next-auth/react";
import { useRouter, useSearchParams } from "next/navigation";
import { useEffect, useMemo, useState } from "react";
import { Input } from "@/components/ui/input";
import { getLanguageColumns } from "./components/language-columns";
import { useDebounce } from "@/hooks/useDebounce";
import useGetLanguages from "@/hooks/useGetLanguage";
import { DataTable } from "@/components/data-table";
import { Skeleton } from "@/components/ui/skeleton";
import FormLanguage from "./components/form-language";
export default function LanguagePage() {
    const { data: session } = useSession();
    const router = useRouter();
    const searchParams = useSearchParams();
    const page = Number(searchParams.get("page")) || 1;
    const search = searchParams.get("search") || undefined;
    const code = searchParams.get("code") || undefined;
    const [searchInput, setSearchInput] = useState(search ?? "");
    const debouncedSearch = useDebounce(searchInput, 500);
    const columns = useMemo(() => getLanguageColumns(), [])
    const { data, isLoading, error } = useGetLanguages({
        page,
        search,
        code
    })
    useEffect(() => {
        const params = new URLSearchParams(searchParams)
        if (debouncedSearch) {
            params.set("search", debouncedSearch)
        } else {
            params.delete("search")
        }
        router.push(`?${params.toString()}`)
    }, [debouncedSearch])
    console.log(data);
    return (
        <>
            <main className="relative mx-auto flex min-h-screen inset-0 w-screen max-w-[1440px] justify-center bg-white dark:bg-black ">
                <section className="relative w-screen h-svh px-10 py-18">
                    <div className="flex flex-col gap-3 sm:flex-row  py-10 md:p-10 sm:items-center justify-between">
                        <h1 className="text-3xl md:text-5xl font-semibold">Linguagens</h1>
                        <div className="flex gap-2">
                            <FormLanguage/>
                            <Input
                                placeholder="Buscar linguagem..."
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
                            <DataTable columns={columns} data={data.items ?? []} />

                        )}
                    </div>
                </section>
            </main>
        </>
    )
}