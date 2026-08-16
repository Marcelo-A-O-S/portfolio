"use client";
import { Button } from "@/components/ui/button";
import { Dialog, DialogClose, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { DropdownMenuItem } from "@/components/ui/dropdown-menu";
import { Field, FieldGroup } from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { linkTypeSchema, LinkTypeSchema } from "@/domain/schemas/LinkTypeSchema";
import { useCreateLinkType } from "@/hooks/LinkType/useCreateLinkType";
import { useUpdateLinkType } from "@/hooks/LinkType/useUpdateLinkType";
import { zodResolver } from "@hookform/resolvers/zod";
import { useEffect } from "react";
import { Controller, useForm } from "react-hook-form";
import { toast } from "sonner";
type FormLinkTypeProps = {
    linkType?: LinkTypeSchema,
    
}
export default function FormLinkType({ linkType }: FormLinkTypeProps) {
    const { mutateAsync: createLinkType } = useCreateLinkType();
    const { mutateAsync: updateLinkType } = useUpdateLinkType();
    const { control, handleSubmit, reset, formState: { errors } } = useForm<LinkTypeSchema>({
        resolver: zodResolver(linkTypeSchema)
    })
    useEffect(() => {
        if (linkType) {
            reset(linkType);
        }
    }, [linkType, reset]);
    const onSubmit = async (data: LinkTypeSchema) => {
        if (linkType) {
            if (!linkType.id)
                return toast.error("O identificador não pode ser nulo.");
            await updateLinkType({ id: linkType.id, data: data });
        } else {
            console.log(data);
            await createLinkType(data);
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
                    {linkType ?
                        <DropdownMenuItem onSelect={(e) => e.preventDefault()} className="cursor-pointer">Update Link Type</DropdownMenuItem>
                        :
                        <Button className="cursor-pointer">Add Link Type</Button>}
                </DialogTrigger>
                <DialogContent>
                    <form onSubmit={handleSubmit(onSubmit)}>
                        <DialogHeader>
                            <DialogTitle>{linkType ? "Edit Link Type" : "Add Link Type"}</DialogTitle>
                            <DialogDescription>
                                {linkType ? `Make changes to your link type here. Click save when you're
                                done.`: `Add your link type here. Click save when you're done.`}
                            </DialogDescription>
                        </DialogHeader>
                        <FieldGroup>
                            <div className="py-3">
                                <Controller
                                    name="name"
                                    control={control}
                                    render={({ field }) => (
                                        <Field className="w-full py-2">
                                            <Label htmlFor="Name">Name</Label>
                                            <Input {...field}
                                                placeholder="Informe o nome" />
                                            {errors.name && <span className="text-sm text-red-600 text-wrap text-justify">{errors.name?.message}</span>}
                                        </Field>
                                    )}
                                />
                                <Controller
                                    name="backgroundColor"
                                    control={control}
                                    render={({ field }) => (
                                        <Field className="w-full py-2">
                                            <Label htmlFor="backgroundColor">Background Color</Label>
                                            <Input {...field}
                                                type="color" />
                                            {errors.backgroundColor && <span className="text-sm text-red-600 text-wrap text-justify">{errors.backgroundColor?.message}</span>}
                                        </Field>
                                    )}
                                />
                                <Controller
                                    name="textColor"
                                    control={control}
                                    render={({ field }) => (
                                        <Field className="w-full py-2">
                                            <Label htmlFor="textColor">Text Color</Label>
                                            <Input {...field}
                                                type="color" />
                                            {errors.textColor && <span className="text-sm text-red-600 text-wrap text-justify">{errors.textColor?.message}</span>}
                                        </Field>
                                    )}
                                />
                                <Controller
                                    name="borderColor"
                                    control={control}
                                    render={({ field }) => (
                                        <Field className="w-full py-2">
                                            <Label htmlFor="borderColor">Border Color</Label>
                                            <Input {...field}
                                                type="color" />
                                            {errors.borderColor && <span className="text-sm text-red-600 text-wrap text-justify">{errors.borderColor?.message}</span>}
                                        </Field>
                                    )}
                                />
                                <Controller
                                    name="icon"
                                    control={control}
                                    render={({ field }) => (
                                        <Field className="w-full py-2">
                                            <Label htmlFor="icon">Icon</Label>
                                            <Input {...field}
                                                placeholder="Informe o icone" />
                                            {errors.icon && <span className="text-sm text-red-600 text-wrap text-justify">{errors.icon?.message}</span>}
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