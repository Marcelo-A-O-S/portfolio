"use client"
import { Button } from "@/components/ui/button";
import { Dialog, DialogClose, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { DropdownMenuItem } from "@/components/ui/dropdown-menu";
import { FieldGroup, Field } from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Select, SelectContent, SelectGroup, SelectItem, SelectLabel, SelectTrigger, SelectValue } from "@/components/ui/select";
import { categorySchema, CategorySchema } from "@/domain/schemas/CategorySchema";
import { useCreateCategory } from "@/hooks/useCreateCategory";
import { useLanguages } from "@/hooks/useLanguages";
import { useUpdateCategory } from "@/hooks/useUpdateCategory";
import { zodResolver } from "@hookform/resolvers/zod";
import { useEffect } from "react";
import { useForm, Controller, useFieldArray } from "react-hook-form";
import { toast } from "sonner";
type FormCategoryProps = {
    category?: CategorySchema
}
export default function FormCategory({ category }: FormCategoryProps) {
    const { mutateAsync: createCategoryAsync } = useCreateCategory();
    const { mutateAsync: updateCategoryAsync } = useUpdateCategory();
    const { data: languages } = useLanguages();
    const { control, handleSubmit, reset, formState: { errors } } = useForm<CategorySchema>({
        resolver: zodResolver(categorySchema),
        defaultValues: {
            categoryContents: [
                {
                    name: "",
                    slug: "",
                    languageId:""
                }
            ]
        }
    })
    const { fields, append, remove, } = useFieldArray({
        control,
        name: "categoryContents"
    })
    useEffect(() => {
        if (category) {
            reset(category);
        }
    }, [category, reset]);
    const onSubmit = async (data: CategorySchema) => {
        if (category) {
            if (!category.id)
                return toast.error("O identificador não pode ser nulo.");
            const response = await updateCategoryAsync({ id: category.id, category: data })
            if (response.status != 200) {
                toast.error(response.data.message);
                return;
            }
            toast.success(response.data.message);
        } else {
            const response = await createCategoryAsync(data);
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
                    {category ?
                        <DropdownMenuItem onSelect={(e) => e.preventDefault()} className="cursor-pointer">Update Category</DropdownMenuItem>
                        :
                        <Button className="cursor-pointer">Add Category</Button>}
                </DialogTrigger>
                <DialogContent className="sm:max-w-sm">
                    <form onSubmit={handleSubmit(onSubmit)}>
                        <DialogHeader >
                            <div className="flex justify-between items-center p-2">
                                <DialogTitle>{category ? "Update Category" : "Create Category"}</DialogTitle>
                                <div>
                                    <Button
                                        type="button"
                                        className="cursor-pointer"
                                        onClick={() =>
                                            append({
                                                name: "",
                                                slug: "",
                                                languageId: ""
                                            })
                                        }
                                    >
                                        Add translation
                                    </Button>
                                </div>
                            </div>
                            <DialogDescription>
                                Gerencie uma categoria:
                            </DialogDescription>
                        </DialogHeader>
                        <FieldGroup className="-mx-4 max-h-[50vh] overflow-y-auto py-2 px-2 w-[350px] overflow-x-hidden">
                            {fields.map((item, index) => (
                                <div key={item.id} className="border p-4 rounded-sm">
                                    <div className="grid grid-cols-2 gap-1 p-1">
                                        <Controller
                                            name={`categoryContents.${index}.languageId`}
                                            control={control}
                                            render={({ field }) => (
                                                <Field className="">
                                                    <Label htmlFor="language">Language</Label>
                                                    <Select
                                                        onValueChange={(value) => field.onChange(value)}
                                                        value={field.value}
                                                    >
                                                        <SelectTrigger className="w-full max-w-48">
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
                                                </Field>
                                            )}
                                        />
                                        <Controller
                                            name={`categoryContents.${index}.slug`}
                                            control={control}
                                            render={({ field }) => (
                                                <Field className="w-full">
                                                    <Label htmlFor="slug">Slug</Label>
                                                    <Input {...field}
                                                        onChange={e => field.onChange(e.target.value)}
                                                        placeholder="Informe o slug" />
                                                    {/* <span className="text-sm text-red-600 text-wrap text-justify">{errors.slug?.message}</span> */}
                                                </Field>
                                            )}
                                        />
                                    </div>
                                    <div className="p-1">
                                        <Controller
                                            name={`categoryContents.${index}.name`}
                                            control={control}
                                            render={({ field }) => (
                                                <Field className="">
                                                    <Label htmlFor="name">Name</Label>
                                                    <Input {...field}
                                                        onChange={e => field.onChange(e.target.value)}
                                                        placeholder="Informe o nome" />
                                                    {/* <span className="text-sm text-red-600 text-wrap text-justify ">{errors.name?.message}</span> */}
                                                </Field>
                                            )}
                                        />
                                    </div>
                                    <div className="flex justify-end p-1">
                                        <Button
                                            className="cursor-pointer"
                                            type="button"
                                            onClick={() => remove(index)}
                                        >
                                            Remover
                                        </Button>
                                    </div>
                                </div>
                            ))}
                        </FieldGroup>
                        <DialogFooter className="px-2">
                            <DialogClose asChild>
                                <Button variant="outline">Cancelar</Button>
                            </DialogClose>
                            <Button className="cursor-pointer" type="submit">Salvar</Button>
                        </DialogFooter>
                    </form>
                </DialogContent>

            </Dialog>
        </>
    )
}