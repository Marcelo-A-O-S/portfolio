"use client";
import { buttonVariants } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { useSession } from "next-auth/react";
import Link from "next/link";
import { useRouter, useSearchParams } from "next/navigation";
import { useState } from "react";
export default function ToolsPage() {
    const { data: session } = useSession();
    const router = useRouter();
    const searchParams = useSearchParams();
    const search = searchParams.get("search") || undefined;
    const [searchInput, setSearchInput] = useState(search ?? "");
    return (
        <main className="relative mx-auto flex min-h-screen inset-0 w-screen max-w-[1440px] justify-center bg-white dark:bg-black ">
            <section className="relative w-full min-h-screen px-10 py-20 flex flex-col">
                <div className="flex flex-col gap-3 sm:flex-row  py-10 md:p-10 sm:items-center justify-between">
                    <h1 className="text-3xl md:text-5xl font-semibold">Ferramentas</h1>
                    <div className="flex gap-2">
                        <Link className={buttonVariants({ variant: "default" }) + ``} href={"/admin/tools/create"}>Add Tool</Link>
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
            </section>
        </main>
    )
}