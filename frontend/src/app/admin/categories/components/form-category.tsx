"use client"
import { Button } from "@/components/ui/button";
import { Dialog, DialogClose, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { FieldGroup, Field } from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { categorySchema, CategorySchema } from "@/domain/schemas/CategorySchema";
import { useCreateCategory } from "@/hooks/useCreateCategory";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm, Controller, useFieldArray } from "react-hook-form";
import { toast } from "sonner";
export default function FormCategory() {
    const { mutateAsync } = useCreateCategory();
    const { control, handleSubmit, formState: { errors } } = useForm<CategorySchema>({
        resolver: zodResolver(categorySchema),
        defaultValues: {
            categoryContents: [
                {
                    language: "",
                    name: "",
                    slug: ""
                }
            ]
        }
    })
    const { fields, append, remove, } = useFieldArray({
        control,
        name: "categoryContents"
    })
    const onSubmit = async (data: CategorySchema) => {
        const response = await mutateAsync(data);
        if(response.status != 200){
            toast.error(response.data.message);
            return;
        }
        toast.success(response.data.message);
    }
    return (
        <>
            <Dialog >
                <DialogTrigger asChild>
                    <Button className="cursor-pointer">Add Category</Button>
                </DialogTrigger>
                <DialogContent className="sm:max-w-sm">
                    <form onSubmit={handleSubmit(onSubmit)}>
                        <DialogHeader >
                            <div className="flex justify-between items-center p-2">
                                <DialogTitle>Create Category</DialogTitle>
                                <div>
                                    <Button
                                        type="button"
                                        className="cursor-pointer"
                                        onClick={() =>
                                            append({
                                                language: "",
                                                name: "",
                                                slug: ""
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
                        <FieldGroup className="-mx-4 max-h-[50vh] overflow-y-auto px-4 w-[350px] overflow-x-hidden">
                            {fields.map((item, index) => (
                                <div key={item.id} className="border p-4 rounded-sm">
                                    <div className="grid grid-cols-2 gap-2 p-1">
                                        <Controller
                                            name={`categoryContents.${index}.language`}
                                            control={control}
                                            render={({ field }) => (
                                                <Field className="">
                                                    <Label htmlFor="Language">Language</Label>
                                                    <Input {...field}
                                                        onChange={e => field.onChange(e.target.value)}
                                                        placeholder="Informe o idioma" />
                                                    {/* <span className="text-sm text-red-600 text-wrap text-justify ">{errors.language?.message}</span> */}
                                                </Field>
                                            )}
                                        />
                                        <Controller
                                            name={`categoryContents.${index}.slug`}
                                            control={control}
                                            render={({ field }) => (
                                                <Field className="w-full">
                                                    <Label htmlFor="username-1">Slug</Label>
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
                                                    <Label htmlFor="username-1">Name</Label>
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
                        <DialogFooter>
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