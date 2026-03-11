"use client"
import { Button } from "@/components/ui/button";
import { Dialog, DialogClose, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { FieldGroup, Field } from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { CategoryContents } from "@/domain/types/CategoryContents";
import { useState } from "react";

export default function FormCategory() {
    const [categoryContents, setCategoryContents] = useState<CategoryContents[]>([]);
    return (
        <>
            <Dialog >
                <form>
                    <DialogTrigger asChild>
                        <Button className="cursor-pointer">Add Category</Button>
                    </DialogTrigger>
                    <DialogContent className="sm:max-w-sm">
                        <DialogHeader >
                            <div className="flex justify-between items-center p-2">
                                <DialogTitle>Create Category</DialogTitle>
                                <div>
                                    <Button className="cursor-pointer">Add</Button>
                                </div>
                            </div>

                        </DialogHeader>
                        <FieldGroup className="-mx-4 no-scrollbar max-h-[50vh] overflow-y-auto px-4 w-[350px]">
                            <div className="grid grid-cols-2 gap-2">
                                <Field className="w-full">
                                    <Label htmlFor="name-1">Language</Label>
                                    <Input id="name-1" name="name" placeholder="Informe o idioma" />
                                </Field>
                                <Field className="w-full">
                                    <Label htmlFor="username-1">Name</Label>
                                    <Input id="username-1" name="username" placeholder="Informe o nome"  />
                                </Field>
                            </div>

                        </FieldGroup>
                        <DialogFooter>
                            <DialogClose asChild>
                                <Button variant="outline">Cancel</Button>
                            </DialogClose>
                            <Button type="submit">Save changes</Button>
                        </DialogFooter>
                    </DialogContent>
                </form>
            </Dialog>
        </>
    )
}