"use client"
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Field, FieldLabel } from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import { InputGroup, InputGroupTextarea } from "@/components/ui/input-group";
import { Label } from "@/components/ui/label";
import { ToolSchema, toolSchema } from "@/domain/schemas/ToolSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import { useState } from "react";
import { Controller, useFieldArray, useForm } from "react-hook-form";
import { Switch } from "@/components/ui/switch"
import MarkdownRenderer from "@/components/markdown-renderer";
import { useLanguages } from "@/hooks/useLanguages";
import { SelectTrigger, SelectValue, SelectContent, SelectGroup, SelectLabel, SelectItem, Select } from "@/components/ui/select";
import { useCategories } from "@/hooks/useCategories";
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog";
import { CategorySchema } from "@/domain/schemas/CategorySchema";
import { useUpdateTool } from "@/hooks/useUpdateTool";
import { useCreateTool } from "@/hooks/useCreateTool";

export default function ToolCreatePage() {
    const { data: categories } = useCategories();
    const { data: languages } = useLanguages();
    const { mutateAsync: updateTool } = useUpdateTool();
    const { mutateAsync: createTool } = useCreateTool();
    const [preview, openPreview] = useState(false);
    const [previewIndex, setPreviewIndex] = useState<number | null>(null);
    const [open, setOpen] = useState(false);
    const { control, handleSubmit, formState: { }, watch } = useForm<ToolSchema>({
        resolver: zodResolver(toolSchema),
        defaultValues: {
            imgUrl: "",
            status: "DRAFT",
            toolContents: [
                {
                    content: "",
                    description: "",
                    name: "",
                    slug: "",
                    languageId: ""
                }
            ],
        }
    });
    const { fields: fieldToolContents, append, remove: removeTool } = useFieldArray({
        control,
        name: "toolContents"
    });
    const { fields: fieldCategories, append: appendCategory, remove } = useFieldArray({
        control,
        name: "categories"
    })
    const categoriesWatch = watch("categories");
    const contents = watch("toolContents");
    const onSubmit = async (data: ToolSchema) => {
        console.log(data);
        await createTool(data);
    }
    const addCategory = (data: CategorySchema) => {
        const exists = fieldCategories.some(
            (c) => c.id === data.id
        )
        if (exists) return
        appendCategory(data);
    }
    const removeCategory = (id: string) => {
        const index = fieldCategories.findIndex(
            (c) => c.id === id
        )
        if (index !== -1) {
            remove(index)
        }
    }
    return (
        <>
            <Dialog open={open} onOpenChange={setOpen}>
                <DialogContent>
                    <DialogHeader>
                        <DialogTitle>Selecionar categorias</DialogTitle>
                    </DialogHeader>
                    <Input placeholder="Buscar categoria..." />
                    <div className="max-h-60 overflow-y-auto">
                        {categories?.map((cat, index) => (
                            <div
                                key={cat.id}
                                className="flex items-center justify-between p-2 hover:bg-muted rounded"
                            >
                                {cat.categoryContents.map((cc, index) => (
                                    <span key={index}>{`${cc.name}`}</span>
                                ))}
                                <Button
                                    size="sm"
                                    className="cursor-pointer"
                                    disabled={categoriesWatch?.some(c => c.id === cat.id)}
                                    onClick={() => addCategory(cat)}
                                >
                                    Adicionar
                                </Button>
                            </div>
                        ))}
                    </div>
                </DialogContent>
            </Dialog>
            <main className="relative mx-auto flex min-h-full inset-0 w-screen max-w-[1440px] justify-center bg-background overflow-x-hidden">
                <section className="relative w-full min-h-screen px-10 py-20 flex flex-col">
                    <div className="flex flex-col gap-3 sm:flex-row py-10 md:p-10 sm:items-center justify-between">
                        <h1 className="text-3xl md:text-5xl font-semibold">Create Tool</h1>
                        <div className="flex gap-2 items-center">
                            <Button type="button"
                                className="cursor-pointer"
                                onClick={() =>
                                    append({
                                        languageId: "",
                                        name: "",
                                        slug: "",
                                        content: "",
                                        description: ""
                                    })
                                } >Add translation</Button>
                        </div>
                    </div>
                    <div className="flex md:p-10">
                        <form onSubmit={handleSubmit(onSubmit,
                            (errors) => {
                                console.log("ERROS:", errors);
                            })} className="flex-1 flex flex-col gap-2 min-h-0">
                            <Card className="">
                                <CardHeader className="flex flex-col md:flex-row md:items-center justify-between">
                                    <CardTitle>Write Post</CardTitle>
                                    <div className="flex  gap-2">
                                        <Controller
                                            name="status"
                                            control={control}
                                            render={({ field }) => (
                                                <Select
                                                    onValueChange={field.onChange}
                                                    value={field.value}
                                                >
                                                    <SelectTrigger className="w-full">
                                                        <SelectValue placeholder="Selecione o tipo de movimentação" />
                                                    </SelectTrigger>
                                                    <SelectContent>
                                                        <SelectGroup>
                                                            <SelectLabel>Status</SelectLabel>
                                                            <SelectItem value="DRAFT">Rascunho</SelectItem>
                                                            <SelectItem value="PUBLISH">Publicado</SelectItem>
                                                            <SelectItem value="ARCHIVED">Arquivado</SelectItem>
                                                        </SelectGroup>
                                                    </SelectContent>
                                                </Select>
                                            )}
                                        />
                                        <Button className="cursor-pointer" type="submit">Save changes</Button>
                                    </div>
                                </CardHeader>
                                <CardContent className="">
                                    <div className="py-2">
                                        <Controller
                                            name={`imgUrl`}
                                            control={control}
                                            render={({ field }) => (
                                                <div className="grid gap-2">
                                                    <Label htmlFor="imgUrl">Imagem</Label>
                                                    <Input
                                                        {...field}
                                                        placeholder="Informe a url ..."
                                                    />
                                                </div>
                                            )}
                                        />
                                    </div>
                                    <div className="flex flex-col gap-2 border-b pb-3 w-full">
                                        <Label>Categorias</Label>
                                        <Button
                                            className="cursor-pointer"
                                            type="button"
                                            variant="outline"
                                            onClick={() => setOpen(true)}
                                        >
                                            + Adicionar categoria
                                        </Button>
                                        <div className="flex flex-wrap gap-2">
                                            {fieldCategories.map((cat) => (
                                                <div
                                                    key={cat.id}
                                                    className="flex items-center gap-2 px-3 py-1 bg-muted rounded-full text-sm"
                                                >
                                                    {cat.categoryContents.map((cc, index) => (
                                                        <span key={index}>{`${cc.name}`}</span>
                                                    ))}
                                                    <button onClick={() => cat.id && removeCategory(cat.id)}>
                                                        ✕
                                                    </button>
                                                </div>
                                            ))}
                                        </div>
                                    </div>

                                    {fieldToolContents.map((item, index) => (
                                        <div key={item.id} className="w-full max-h-full h-[500px] flex flex-col overflow-hidden">
                                            <div className="flex flex-col gap-6 flex-1 min-h-0 border-t pb-3 py-3">
                                                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 md:gap-2">
                                                    <Controller
                                                        name={`toolContents.${index}.name`}
                                                        control={control}
                                                        render={({ field }) => (
                                                            <Field className="grid gap-2">
                                                                <Label htmlFor="title">Name</Label>
                                                                <Input
                                                                    {...field}
                                                                    placeholder="Informe o nome..."
                                                                />
                                                            </Field>
                                                        )}
                                                    />
                                                    <Controller
                                                        name={`toolContents.${index}.slug`}
                                                        control={control}
                                                        render={({ field }) => (
                                                            <Field className="grid gap-2">
                                                                <Label htmlFor="title">Slug</Label>
                                                                <Input
                                                                    {...field}
                                                                    placeholder="Informe a URL... "
                                                                />
                                                            </Field>
                                                        )}
                                                    />
                                                    <Controller
                                                        name={`toolContents.${index}.languageId`}
                                                        control={control}
                                                        render={({ field }) => (
                                                            <Field className="grid gap-2">
                                                                <Label htmlFor="language">Language</Label>
                                                                <Select

                                                                    onValueChange={(value) => field.onChange(value)}
                                                                    value={field.value}
                                                                >
                                                                    <SelectTrigger className="w-full ">
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
                                                </div>
                                                <Controller
                                                    name={`toolContents.${index}.description`}
                                                    control={control}
                                                    render={({ field }) => (
                                                        <div className="grid gap-2">
                                                            <Label htmlFor="description">Description</Label>
                                                            <Input
                                                                {...field}
                                                                placeholder="Informe a descrição..."
                                                            />
                                                        </div>
                                                    )}
                                                />
                                                <Controller
                                                    name={`toolContents.${index}.content`}
                                                    control={control}
                                                    render={({ field }) => (
                                                        <Field className="flex flex-col flex-1 min-h-0">
                                                            <FieldLabel htmlFor="block-start-textarea">Content</FieldLabel>
                                                            <InputGroup className="flex-1 min-h-0 items-stretch">
                                                                <InputGroupTextarea
                                                                    {...field}
                                                                    placeholder="Informe o conteudo aqui..."
                                                                    className="flex-1 resize-none overflow-y-auto text-sm leading-relaxed"
                                                                />
                                                            </InputGroup>
                                                        </Field>
                                                    )}
                                                />
                                                <div className="flex justify-end">
                                                    <Button
                                                        className="cursor-pointer"
                                                        type="button"
                                                        variant="destructive"
                                                        onClick={() => removeTool(index)}
                                                    >
                                                        Remove
                                                    </Button>
                                                </div>
                                            </div>
                                        </div>
                                    ))}
                                </CardContent>
                            </Card>
                        </form>
                    </div>
                </section>
            </main >
        </>
    )
}