"use client";
import { Badge } from "@/components/ui/badge";
import { Button, buttonVariants } from "@/components/ui/button";
import Link from "next/link";
import { Card, CardAction, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Heart, MessageCircle } from 'lucide-react';
import { useSearchParams } from "next/navigation";
import { useState } from "react";
import { Input } from "@/components/ui/input";
export default function ProjectPage() {
    const searchParams = useSearchParams();
    const page = Number(searchParams.get("page")) || 1;
    const search = searchParams.get("search") || undefined;
    const [searchInput, setSearchInput] = useState(search ?? "");
    return (
        <>
            <main className="relative mx-auto flex min-h-screen inset-0 w-screen max-w-[1440px] justify-center bg-white dark:bg-black ">
                <section className="relative w-screen h-svh px-10 py-18">
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
                            <div className="bg-background border border-primary max-w-xl w-full rounded-lg overflow-hidden shadow-sm">
                                <article className="p-4 flex space-x-3">
                                    <div className="flex-1 min-w-0">
                                        <div className="flex justify-between items-center mb-1">
                                            <div className="flex items-baseline space-x-1 text-sm min-w-0">
                                                <span className="font-bold text-primary truncate hover:underline cursor-pointer">
                                                    Title project
                                                </span>
                                                <span className="text-primary hover:underline cursor-pointer whitespace-nowrap">
                                                    3h
                                                </span>
                                            </div>
                                        </div>
                                        <p className="text-primary text-sm leading-normal mb-2">
                                            Lorem ipsum dolor sit amet consectetur adipisicing elit. Accusamus dolorum eos, inventore, sequi commodi non repudiandae maiores enim dolores vero cumque assumenda blanditiis aut, fuga consectetur quisquam tempora placeat a.
                                        </p>
                                        <div>
                                            <Badge variant="secondary">Featured</Badge>
                                        </div>
                                        <div className="mt-3 rounded-xl border tweet-border overflow-hidden">
                                            <img src="https://images.unsplash.com/photo-1605379399642-870262d3d051?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=870&q=80"
                                                alt="Tweet media content" className="w-full object-cover aspect-video" />
                                        </div>
                                        <div className="flex  items-center mt-4 text-primary text-xs sm:text-sm">
                                            <button className="flex items-center space-x-1 p-2 rounded-full">
                                                <Heart />
                                                <span>15</span>
                                            </button>
                                            <button className="flex items-center space-x-1 p-2 rounded-full">
                                                <MessageCircle />
                                                <span>15</span>
                                            </button>
                                        </div>
                                        <Button className="w-full">View Event</Button>
                                    </div>
                                </article>
                            </div>
                            <div className="bg-background border border-primary max-w-xl w-full rounded-lg overflow-hidden shadow-sm">
                                <article className="p-4 flex space-x-3">
                                    <div className="flex-1 min-w-0">
                                        <div className="flex justify-between items-center mb-1">
                                            <div className="flex items-baseline space-x-1 text-sm min-w-0">
                                                <span className="font-bold text-primary truncate hover:underline cursor-pointer">
                                                    Title project
                                                </span>
                                                <span className="text-primary hover:underline cursor-pointer whitespace-nowrap">
                                                    3h
                                                </span>
                                            </div>
                                        </div>
                                        <p className="text-primary text-sm leading-normal mb-2">
                                            Lorem ipsum dolor sit amet consectetur adipisicing elit. Accusamus dolorum eos, inventore, sequi commodi non repudiandae maiores enim dolores vero cumque assumenda blanditiis aut, fuga consectetur quisquam tempora placeat a.
                                        </p>
                                        <div>
                                            <Badge variant="secondary">Featured</Badge>
                                        </div>
                                        <div className="mt-3 rounded-xl border tweet-border overflow-hidden">
                                            <img src="https://images.unsplash.com/photo-1605379399642-870262d3d051?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=870&q=80"
                                                alt="Tweet media content" className="w-full object-cover aspect-video" />
                                        </div>
                                        <div className="flex  items-center mt-4 text-primary text-xs sm:text-sm">
                                            <button className="flex items-center space-x-1 p-2 rounded-full">
                                                <Heart />
                                                <span>15</span>
                                            </button>
                                            <button className="flex items-center space-x-1 p-2 rounded-full">
                                                <MessageCircle />
                                                <span>15</span>
                                            </button>
                                        </div>
                                        <Button className="w-full">View Event</Button>
                                    </div>
                                </article>
                            </div>
                            <div className="bg-background border border-primary max-w-xl w-full rounded-lg overflow-hidden shadow-sm">
                                <article className="p-4 flex space-x-3">
                                    <div className="flex-1 min-w-0">
                                        <div className="flex justify-between items-center mb-1">
                                            <div className="flex items-baseline space-x-1 text-sm min-w-0">
                                                <span className="font-bold text-primary truncate hover:underline cursor-pointer">
                                                    Title project
                                                </span>
                                                <span className="text-primary hover:underline cursor-pointer whitespace-nowrap">
                                                    3h
                                                </span>
                                            </div>
                                        </div>
                                        <p className="text-primary text-sm leading-normal mb-2">
                                            Lorem ipsum dolor sit amet consectetur adipisicing elit. Accusamus dolorum eos, inventore, sequi commodi non repudiandae maiores enim dolores vero cumque assumenda blanditiis aut, fuga consectetur quisquam tempora placeat a.
                                        </p>
                                        <div>
                                            <Badge variant="secondary">Featured</Badge>
                                        </div>
                                        <div className="mt-3 rounded-xl border tweet-border overflow-hidden">
                                            <img src="https://images.unsplash.com/photo-1605379399642-870262d3d051?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=870&q=80"
                                                alt="Tweet media content" className="w-full object-cover aspect-video" />
                                        </div>
                                        <div className="flex  items-center mt-4 text-primary text-xs sm:text-sm">
                                            <button className="flex items-center space-x-1 p-2 rounded-full">
                                                <Heart />
                                                <span>15</span>
                                            </button>
                                            <button className="flex items-center space-x-1 p-2 rounded-full">
                                                <MessageCircle />
                                                <span>15</span>
                                            </button>
                                        </div>
                                        <Button className="w-full">View Event</Button>
                                    </div>
                                </article>
                            </div>
                        </div>

                    </div>
                </section>
            </main>
        </>
    )
}