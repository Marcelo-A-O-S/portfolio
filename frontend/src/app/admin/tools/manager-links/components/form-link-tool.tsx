"use client";
import { Button, buttonVariants } from "@/components/ui/button";
import { Dialog, DialogClose, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { DropdownMenuItem } from "@/components/ui/dropdown-menu";
import { Field, FieldGroup } from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Select, SelectContent, SelectGroup, SelectItem, SelectLabel, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Separator } from "@/components/ui/separator";
import { linkSchema, LinkSchema } from "@/domain/schemas/LinkSchema"
import { useLanguages } from "@/hooks/Language/useLanguages";
import { useCreateLink } from "@/hooks/Link/useCreateLink"
import { useUpdateLink } from "@/hooks/Link/useUpdateLink";
import { useGetLinkTypes } from "@/hooks/LinkType/useGetLinkTypes";
import { useAddLinkTool } from "@/hooks/Tool/Link/useAddLinkTool";
import { useUpdateLinkTool } from "@/hooks/Tool/Link/useUpdateLinkTool";
import { zodResolver } from "@hookform/resolvers/zod";
import { XIcon } from "lucide-react";
import Image from "next/image";
import Link from "next/link";
import { useEffect } from "react";
import { Controller, useFieldArray, useForm } from "react-hook-form";
import { toast } from "sonner";
type FormLinkProps = {
    link?: LinkSchema,
    toolId: string
}
export default function FormLinkTool({ link, toolId }: FormLinkProps) {
    const { data: linkTypes } = useGetLinkTypes();
    const { data: languages } = useLanguages();
    const { mutateAsync: createLink, isPending: isAdding } = useAddLinkTool();
    const { mutateAsync: updateLink, isPending: isUpdating } = useUpdateLinkTool();
    const { control, handleSubmit, reset, formState: { errors } } = useForm<LinkSchema>({
        resolver: zodResolver(linkSchema),
        defaultValues: {
            toolId: toolId,
            descriptions: [
                {
                    title: "",
                    languageId: ""
                }
            ]
        }
    })
    const { fields, append, remove, } = useFieldArray({
        control,
        name: "descriptions"
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
    const isSubmitting = isAdding || isUpdating;
    return (
        <>
            <Dialog
                onOpenChange={(open) => {
                    if (!open) reset()
                }}
            >
                <DialogTrigger asChild>
                    {link ?
                        <DropdownMenuItem
                            onSelect={(e) => e.preventDefault()}
                            className="cursor-pointer">Update Link</DropdownMenuItem>
                        :
                        <Button className="cursor-pointer">Add Link</Button>}
                </DialogTrigger>
                <DialogContent className="flex max-h-[85vh] w-[95vw] max-w-2xl flex-col gap-0 overflow-hidden p-0">
                    <form onSubmit={handleSubmit(onSubmit)}
                        className="flex min-h-0 flex-1 flex-col"
                    >
                        <DialogHeader className="shrink-0 border-b px-6 py-4">
                            <DialogTitle>{link ? "Edit Link" : "Add Link"}</DialogTitle>
                            <DialogDescription>
                                {link ? `Make changes to your link here. Click save when you're
                                done.`: `Add your link here. Click save when you're done.`}
                            </DialogDescription>
                        </DialogHeader>
                        <div className="flex min-h-0 flex-1 flex-col md:flex-row gap-6 overflow-y-auto px-6 py-5">
                            <FieldGroup className="">
                                <Controller
                                    name="url"
                                    control={control}
                                    render={({ field }) => (
                                        <Field >
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
                                        <Field >
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
                                <div className="flex flex-col gap-2">
                                    <Label>Descriptions</Label>
                                    <Button
                                        type="button"
                                        className="cursor-pointer"
                                        onClick={() =>
                                            append({
                                                title: "",
                                                languageId: ""
                                            })
                                        }
                                    >
                                        Add Description
                                    </Button>
                                </div>
                            </FieldGroup>
                            <Separator
                                orientation="vertical"
                                className="hidden  md:block"
                            />
                            <FieldGroup className="flex flex-col gap-2 overflow-y-auto rounded-xl border p-2">
                                {fields.map((item, index) => (
                                    <>
                                        <div className="bg-muted/30 flex items-start gap-2 rounded-lg border p-2">
                                            <div className="flex flex-col gap-2">
                                                <Controller
                                                    name={`descriptions.${index}.languageId`}
                                                    control={control}
                                                    render={({ field }) => (
                                                        <Select
                                                            onValueChange={(value) => field.onChange(value)}
                                                            value={field.value}
                                                        >
                                                            <SelectTrigger className="w-full min-w-0">
                                                                <SelectValue placeholder="Selecione o idioma" />
                                                            </SelectTrigger>
                                                            <SelectContent>
                                                                <SelectGroup>
                                                                    <SelectLabel>Idiomas</SelectLabel>
                                                                    {languages?.map((item, index) => (
                                                                        <SelectItem key={index} value={`${item.id}`}>{item.name}</SelectItem>
                                                                    ))}
                                                                </SelectGroup>
                                                            </SelectContent>
                                                        </Select>
                                                    )}
                                                />
                                                <Controller
                                                    control={control}
                                                    name={`descriptions.${index}.title`}
                                                    render={({ field }) => (
                                                        <Input {...field}
                                                            className="min-w-0"
                                                            placeholder="Informe o titulo" />
                                                    )}
                                                />
                                            </div>
                                            <Button
                                                type="button"
                                                variant="ghost"
                                                size="icon"
                                                className="text-muted-foreground hover:text-destructive size-8 shrink-0 cursor-pointer"
                                                onClick={() => remove(index)}
                                                disabled={fields.length === 1}
                                            >
                                                <XIcon className="size-4" />
                                            </Button>
                                        </div>
                                    </>
                                ))}
                            </FieldGroup>
                        </div>
                        <DialogFooter className="shrink-0 border-t px-6 py-4">
                            <DialogClose asChild>
                                <Button className="cursor-pointer" variant="outline">Cancel</Button>
                            </DialogClose>
                            <Button
                                className="cursor-pointer"
                                type="submit"
                                disabled={isSubmitting}
                            >
                                {link?`Update`:`Save`}
                            </Button>
                        </DialogFooter>
                    </form>
                </DialogContent>
            </Dialog>
        </>
    )
}