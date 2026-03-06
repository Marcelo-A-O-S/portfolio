"use client";
import { buttonVariants } from "@/components/ui/button";
import Link from "next/link";
import { DataTable } from "./data-table";
import { useRouter, useSearchParams } from "next/navigation";
import { useEffect, useState } from "react";
import { User } from "@/domain/types/User";
import { getUsersByPaginationService } from "@/services/client/user-services";
import { toast } from "sonner";
import { ColumnDef } from "@tanstack/react-table";
import { getUsersColumns } from "./user-columns";

export default function UsersPage(){
    const router = useRouter();
    const searchParams = useSearchParams();
    const pageValue = searchParams.get("page");
    const page = pageValue ? parseInt(pageValue) : 1;
    const [ users, setUsers] = useState<User[]>([])
    const [ isLoading, setIsLoading ] = useState(false);
    const [ columns, setColumns ] = useState<ColumnDef<User>[]>([]);
    useEffect(()=>{
        getUsers();
    },[])
    useEffect(()=>{
        getUsers();
    },[page])
    const getUsers = async() =>{
        setIsLoading(true);
        try{
            const response = await getUsersByPaginationService(page);
            if(response.status !== 200 && response.status !== 201){
                return toast.error(response.data.message);
            }
            setColumns(getUsersColumns);
            setUsers(response.data);
            console.log(response.data);
        }finally{
            setIsLoading(false);
        }
    }
    return(
        <main className="relative mx-auto flex min-h-screen inset-0 w-screen max-w-[1440px] justify-center bg-white dark:bg-black ">
            <section className="relative w-screen h-svh px-10 py-20 flex flex-col">
                <div className="flex p-10 items-center justify-between">
                    <h1 className="text-3xl md:text-5xl">Usuários</h1>
                    <div className="flex gap-2 items-center">
                        <Link className={buttonVariants({ variant: "default" }) + ``} href={"/admin/tools/create"}>Add Tool</Link>
                    </div>
                </div>
                <div className="p-10">
                    <DataTable columns={columns} data={users} />
                </div>
            </section>
        </main>
    )
}