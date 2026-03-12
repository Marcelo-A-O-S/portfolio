"use client"
import { Button } from "@/components/ui/button";
import { Dialog, DialogClose, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { FieldGroup, Field } from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { CategoryContentSchema, categoryContentSchema } from "@/domain/schemas/CategoryContentSchema";
import { CategoryContents } from "@/domain/types/CategoryContents";
import { zodResolver } from "@hookform/resolvers/zod";
import { useState } from "react";
import { useForm, Controller } from "react-hook-form";

export default function FormCategory() {
    const [categoryContents, setCategoryContents] = useState<CategoryContents[]>([]);
    const { control, formState: { errors } } = useForm<CategoryContentSchema>({
        resolver: zodResolver(categoryContentSchema),
        defaultValues: {
            name: "",
            language: "",
            slug: ""
        }
    })
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
                            <DialogDescription>
                                Gerencie uma categoria:
                            </DialogDescription>
                        </DialogHeader>
                        <FieldGroup className="-mx-4 max-h-[50vh] overflow-y-auto px-4 w-[350px] overflow-x-hidden">
                            <div className="border p-4 rounded-sm">
                                <div className="grid grid-cols-2 gap-2">
                                    <Controller
                                        name="language"
                                        control={control}
                                        render={({ field }) => (
                                            <Field className="">
                                                <Label htmlFor="Language">Language</Label>
                                                <Input {...field} placeholder="Informe o idioma" />
                                                <span className="text-sm text-red-600 text-wrap text-justify ">{errors.language?.message}</span>
                                            </Field>
                                        )}
                                    />
                                    <Controller
                                        name="slug"
                                        control={control}
                                        render={({ field }) => (
                                            <Field className="w-full">
                                                <Label htmlFor="username-1">Slug</Label>
                                                <Input {...field} placeholder="Informe o slug" />
                                                <span className="text-sm text-red-600 text-wrap text-justify">{errors.slug?.message}</span>
                                            </Field>
                                        )}
                                    />
                                </div>
                                <Controller
                                    name="name"
                                    control={control}
                                    render={({ field }) => (
                                        <Field className="w-full">
                                            <Label htmlFor="username-1">Name</Label>
                                            <Input {...field} placeholder="Informe o nome" />
                                            <span className="text-sm text-red-600 text-wrap text-justify ">{errors.name?.message}</span>
                                        </Field>
                                    )}
                                />
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