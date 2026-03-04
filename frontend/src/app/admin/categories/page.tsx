import { buttonVariants } from "@/components/ui/button";
import Link from "next/link";
export default function ToolsPage() {

    return (
        <main className="relative mx-auto flex min-h-screen inset-0 w-screen max-w-[1440px] justify-center bg-white dark:bg-black ">
            <section className="relative w-screen h-svh px-10 py-18">
                <div className="flex p-10 items-center justify-between">
                    <h1 className="text-3xl md:text-5xl">Categories</h1>
                    <div className="flex gap-2 items-center">
                        <Link className={buttonVariants({ variant: "default" }) + ``} href={"/admin/categories/create"}>Add Category</Link>
                    </div>
                </div>
            </section>
        </main>
    )
}