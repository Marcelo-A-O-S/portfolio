import { Avatar, AvatarImage } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";
import { SocialAccounts } from "@/domain/types/SocialAccounts";
import { useEffect, useRef, useState } from "react";
import Link from "next/link";
interface DialogAccountsProps {
    socialAccounts: SocialAccounts[]
}
export default function DialogAccounts({ socialAccounts }: DialogAccountsProps) {
    const dialogRef = useRef<HTMLDialogElement | null>(null)
    const [isOpen, setIsOpen] = useState(false);
    useEffect(() => {
        if (!dialogRef.current) return;
        if (isOpen) {
            dialogRef.current.showModal();
        } else {
            dialogRef.current.close();
        }
    }, [isOpen])
    return (
        <>
            <dialog ref={dialogRef} className="hidden open:flex fixed left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2  items-center justify-center bg-transparent">
                <div className="relative h-full w-full border-2 rounded-2xl
                    border-primary dark:bg-black
                    ">
                    <div className="flex flex-col gap-3 justify-center p-5 md:p-10">
                        <div className="flex flex-col gap-1">
                            <h1 className="text-lg font-semibold">Contas Associadas</h1>
                            <hr />
                        </div>
                        <div className="flex flex-col gap-1">
                            <Card className="w-[250px] md:w-[350px] ">
                                {socialAccounts.map((account, index) => {
                                    return (
                                        <CardContent key={account.id} className="flex items-center gap-4 py-3">
                                            <Avatar>
                                                <AvatarImage
                                                    src={account.profileUrl}
                                                    alt={account.username}
                                                />
                                            </Avatar>

                                            <div className="flex flex-col">
                                                <span className="text-sm font-medium">
                                                    <Link href={""}> {account.username}</Link>
                                                </span>

                                                <span className="text-xs text-muted-foreground">
                                                    {account.provider}
                                                </span>
                                            </div>
                                        </CardContent>
                                    )
                                })}

                            </Card>
                        </div>
                        <div className="flex justify-end">
                            <hr />
                            <button onClick={() => setIsOpen(false)} className="border-2 rounded-sm border-primary flex items-center justify-center py-1 px-2 cursor-pointer">Fechar</button>
                        </div>
                    </div>
                </div>
            </dialog>
            <button onClick={() => setIsOpen(true)} className="cursor-pointer ">Contas Associadas</button>
        </>
    )
}