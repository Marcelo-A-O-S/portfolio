"use client";
import { Badge } from "@/components/ui/badge";
import { Button, buttonVariants } from "@/components/ui/button";
import Link from "next/link";
import { Card, CardAction, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Heart, MessageCircle } from 'lucide-react';
import { useSearchParams } from "next/navigation";
import { useState } from "react";
import { Input } from "@/components/ui/input";
import { useLanguages } from "@/hooks/Language/useLanguages";
import { useSession } from "next-auth/react";
import { usePaginationProject } from "@/hooks/Post/usePaginationProject";
import CardProject from "./components/card-project";
export default function ProjectPage() {
    const { data: session } = useSession();
    const searchParams = useSearchParams();
    const page = Number(searchParams.get("page")) || 1;
    const search = searchParams.get("search") || undefined;
    const [searchInput, setSearchInput] = useState(search ?? "");
    const { data: languages } = useLanguages();
    const { data: projects } = usePaginationProject({
        page,
        search
    })
    // console.log("PROJECTS: ", projects);
    return (
        <>
            <main className="relative mx-auto flex min-h-screen inset-0 w-full max-w-[1440px] justify-center bg-white dark:bg-black ">
                <section className="relative w-full min-h-screen px-10 py-18">
                    <div className="flex p-10 items-center justify-between">
                        <h1 className="text-3xl md:text-5xl">Projects</h1>
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
                    <div className="flex py-10 md:p-10">
                        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 p-5">
                            {projects?.items.map((item, index)=>(
                                <CardProject key={index} item={item} languages={languages} />
                            ))}
                        </div>

                    </div>
                </section>
            </main>
        </>
    )
}