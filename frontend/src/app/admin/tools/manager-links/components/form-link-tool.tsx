"use client";
import { Button, buttonVariants } from "@/components/ui/button";
import { Dialog, DialogClose, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { DropdownMenuItem } from "@/components/ui/dropdown-menu";
import { Field, FieldGroup } from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Select, SelectContent, SelectGroup, SelectItem, SelectLabel, SelectTrigger, SelectValue } from "@/components/ui/select";
import { linkSchema, LinkSchema } from "@/domain/schemas/LinkSchema"
import { useCreateLink } from "@/hooks/Link/useCreateLink"
import { useUpdateLink } from "@/hooks/Link/useUpdateLink";
import { useGetLinkTypes } from "@/hooks/LinkType/useGetLinkTypes";
import { useAddLinkTool } from "@/hooks/Tool/Link/useAddLinkTool";
import { useUpdateLinkTool } from "@/hooks/Tool/Link/useUpdateLinkTool";
import { zodResolver } from "@hookform/resolvers/zod";
import Image from "next/image";
import Link from "next/link";
import { useEffect } from "react";
import { Controller, useForm } from "react-hook-form";
import { toast } from "sonner";
type FormLinkProps = {
    link?: LinkSchema,
    toolId: string
}
export default function FormLinkTool({ link, toolId }: FormLinkProps) {
    const { data: linkTypes } = useGetLinkTypes();
    const { mutateAsync: createLink, isPending: isAdding } = useAddLinkTool();
    const { mutateAsync: updateLink, isPending: isUpdating } = useUpdateLinkTool();
    const { control, handleSubmit, reset, formState: { errors } } = useForm<LinkSchema>({
        resolver: zodResolver(linkSchema),
        defaultValues: {
            toolId: toolId
        }
    })
    useEffect(() => {
        if (link) {
            reset(link);
        }
    }, [link, reset]);
    const onSubmit = async (data: LinkSchema) => {
        if (link) {
            if (!link.id)
                return toast.error("O identificador não pode ser nulo.");
            updateLink({ id: link.id, data: data })
        } else {
            createLink(data)
        }
    }
    return (
        <>
            <Dialog
                onOpenChange={(open) => {
                    if (!open) reset()
                }}
            >
                <DialogTrigger asChild>
                    {link ?
                        <DropdownMenuItem onSelect={(e) => e.preventDefault()} className="cursor-pointer">Update Link</DropdownMenuItem>
                        :
                        <Button className="cursor-pointer">Add Link</Button>}
                </DialogTrigger>
                <DialogContent>
                    <form onSubmit={handleSubmit(onSubmit)}>
                        <DialogHeader>
                            <DialogTitle>{link ? "Edit Link" : "Add Link"}</DialogTitle>
                            <DialogDescription>
                                {link ? `Make changes to your link here. Click save when you're
                                done.`: `Add your link here. Click save when you're done.`}
                            </DialogDescription>
                        </DialogHeader>
                        <FieldGroup>
                            <div className="py-3">
                                <Controller
                                    name="title"
                                    control={control}
                                    render={({ field }) => (
                                        <Field className="w-full py-2">
                                            <Label htmlFor="Title">Title</Label>
                                            <Input {...field}
                                                placeholder="Informe o titulo do link" />
                                            {errors.title && <span className="text-sm text-red-600 text-wrap text-justify">{errors.title?.message}</span>}
                                        </Field>
                                    )}
                                />
                                <Controller
                                    name="url"
                                    control={control}
                                    render={({ field }) => (
                                        <Field className="w-full py-2">
                                            <Label htmlFor="Url">Url</Label>
                                            <Input {...field}
                                                placeholder="Informe o url do link" />
                                            {errors.url && <span className="text-sm text-red-600 text-wrap text-justify">{errors.url?.message}</span>}
                                        </Field>
                                    )}
                                />
                                <Controller
                                    name={`linkTypeId`}
                                    control={control}
                                    render={({ field }) => (
                                        <Field className="">
                                            <Label htmlFor="linkType">Link Type</Label>
                                            <Select
                                                onValueChange={(value) => field.onChange(value)}
                                                value={field.value}
                                            >
                                                <SelectTrigger className="w-full">
                                                    <SelectValue placeholder="Selecione o tipo de link" />
                                                </SelectTrigger>
                                                <SelectContent>
                                                    <SelectGroup>
                                                        <SelectLabel>Link Types</SelectLabel>
                                                        {linkTypes?.map((linkType, index) => (
                                                            <SelectItem key={index} value={`${linkType.id}`}>
                                                                <Link
                                                                    href={""}
                                                                    style={{
                                                                        backgroundColor: linkType.backgroundColor,
                                                                        color: linkType.textColor,
                                                                        borderColor: linkType.borderColor,
                                                                        border: "1px",
                                                                        borderStyle: "solid"
                                                                    }}
                                                                    className={`${buttonVariants({ variant: "default" })} cursor-pointer`}
                                                                >
                                                                    <Image src={linkType.icon} alt={linkType.name} width={20} height={20} />
                                                                    {linkType.name}
                                                                </Link>
                                                            </SelectItem>
                                                        ))}
                                                    </SelectGroup>
                                                </SelectContent>
                                            </Select>
                                        </Field>
                                    )}
                                />
                            </div>
                        </FieldGroup>
                        <DialogFooter>
                            <DialogClose asChild>
                                <Button className="cursor-pointer" variant="outline">Cancel</Button>
                            </DialogClose>
                            <Button className="cursor-pointer" type="submit">Save changes</Button>
                        </DialogFooter>
                    </form>
                </DialogContent>
            </Dialog>
        </>
    )
}