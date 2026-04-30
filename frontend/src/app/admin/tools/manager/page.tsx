"use client"
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Field, FieldLabel } from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import { InputGroup, InputGroupTextarea } from "@/components/ui/input-group";
import { Label } from "@/components/ui/label";
import { ToolSchema, toolSchema } from "@/domain/schemas/ToolSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import { useEffect, useState } from "react";
import { Controller, ControllerRenderProps, useFieldArray, useForm } from "react-hook-form";
import { useLanguages } from "@/hooks/useLanguages";
import { SelectTrigger, SelectValue, SelectContent, SelectGroup, SelectLabel, SelectItem, Select } from "@/components/ui/select";
import { useCategories } from "@/hooks/useCategories";
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog";
import { CategorySchema } from "@/domain/schemas/CategorySchema";
import { useUpdateTool } from "@/hooks/useUpdateTool";
import { useCreateTool } from "@/hooks/useCreateTool";
import { useRouter, useSearchParams } from "next/navigation";
import { useGetByIdTool } from "@/hooks/useGetByIdTool";
import { addImageMarkDown } from "@/services/client/image-client";
import { toast } from "sonner";
export default function ToolCreatePage() {
    const searchParams = useSearchParams();
    const toolId = searchParams.get("toolId") || undefined;
    const { data: categories } = useCategories();
    const { data: languages } = useLanguages();
    const { data: tool } = useGetByIdTool(toolId)
    const { mutateAsync: updateTool } = useUpdateTool();
    const { mutateAsync: createTool } = useCreateTool();
    const [preview, openPreview] = useState(false);
    const [previewImage, setImagePreview ] = useState(false);
    const [open, setOpen] = useState(false);
    const { control, handleSubmit, formState: { errors: errorsTool }, watch, reset, getValues, setValue } = useForm<ToolSchema>({
        resolver: zodResolver(toolSchema),
        defaultValues: {
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
    useEffect(() => {
        if (!tool) return;
        //URL.createObjectURL(tool.imgUrl);
        reset(tool)
    }, [tool, reset]);
    const { fields: fieldToolContents, append, remove: removeTool } = useFieldArray({
        control,
        name: "toolContents"
    });
    const { append: appendCategory, remove: removeFieldCategory } = useFieldArray({
        control,
        name: "categories"
    })
    console.log("Update Tool: ",tool);
    const categoriesWatch = watch("categories");
    const onSubmit = async (data: ToolSchema) => {
        if (tool) {
            await updateTool({ id: tool.id, data: data });
        } else {
            await createTool(data);
        }
    }
    const addCategory = (data: CategorySchema) => {
        const exists = categoriesWatch.some(
            (c) => c.id === data.id
        )
        if (exists) return
        appendCategory(data);
    }
    const handleDrop = async (e: React.DragEvent<HTMLTextAreaElement>, field: ControllerRenderProps<ToolSchema, `toolContents.${number}.content`>, index: number) => {
        e.preventDefault();
        const file = e.dataTransfer.files?.[0];
        if (!file || !file.type.startsWith("image/")) return;
        const textarea = e.currentTarget;
        const start = textarea.selectionStart;
        const end = textarea.selectionEnd;
        const response = await addImageMarkDown(file);
        if (response.status !== 200 && response.status !== 201) {
            toast.error(`Erro ao adicionar imagem: ${response.data.message}`);
            return;
        }
        const url = response.data.url;
        const urlMarkdown = `\n![image](${url})\n`
        const newValue = field.value.substring(0, start) + urlMarkdown + field.value.substring(end);
        field.onChange(newValue);
        const currentImages = getValues(`toolContents.${index}.imagesUrls`) ?? [];
        setValue(`toolContents.${index}.imagesUrls`, [...currentImages, url]);
        const imagesUpdated = getValues(`toolContents.${index}.imagesUrls`);
        console.log(imagesUpdated);
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
                        {categories?.map((cat) => (
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
            <main className="relative mx-auto flex min-h-full inset-0 w-full max-w-[1440px] justify-center ">
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
                        <form onSubmit={handleSubmit(onSubmit)} className="flex-1 flex flex-col gap-2 min-h-0">
                            <Card className="">
                                <CardHeader className="flex flex-col md:flex-row md:items-center justify-between">
                                    <CardTitle>Write Tool</CardTitle>
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
                                                    <div className="flex flex-col gap-2">
                                                        <Label htmlFor="imgUrl">Imagem</Label>
                                                        <Input
                                                            onChange={(e) => field.onChange(e.target.files?.[0])}
                                                            type="file"
                                                            className="cursor-pointer"
                                                        />
                                                    </div>
                                                    {errorsTool.imgUrl && <span className="text-wrap text-red-600 text-sm">{errorsTool.imgUrl.message}</span>}
                                                </div>
                                            )}
                                        />
                                    </div>
                                    <div className="flex flex-col gap-2 pb-3 w-full">
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
                                            <div className="flex gap-2">
                                                {categoriesWatch?.map((cat, index) => (
                                                    <div
                                                        key={cat.id}
                                                        className="flex items-center gap-2 px-3 py-1 bg-muted rounded-full text-sm"
                                                    >
                                                        {cat.categoryContents.map((cc, index) => (
                                                            <span key={index}>{`${cc.name}`}</span>
                                                        ))}
                                                        <button type="button" className="cursor-pointer" onClick={() => removeFieldCategory(index)}>
                                                            ✕
                                                        </button>
                                                    </div>
                                                ))}
                                            </div>
                                                {errorsTool.categories && <span className="text-wrap text-red-600 text-sm">{errorsTool.categories.message}</span>}
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
                                                                <div className="flex flex-col gap-2">
                                                                    <Label htmlFor="title">Name</Label>
                                                                    <Input
                                                                        {...field}
                                                                        placeholder="Informe o nome..."
                                                                    />
                                                                    {errorsTool.toolContents?.[index]?.name && <span className="text-wrap text-red-600 text-sm">{errorsTool.toolContents[index]?.name?.message}</span>}
                                                                </div>
                                                            </Field>
                                                        )}
                                                    />
                                                    <Controller
                                                        name={`toolContents.${index}.slug`}
                                                        control={control}
                                                        render={({ field }) => (
                                                            <Field className="grid gap-2">
                                                                <div className="flex flex-col gap-2">
                                                                    <Label htmlFor="title">Slug</Label>
                                                                    <Input
                                                                        {...field}
                                                                        placeholder="Informe a URL... "
                                                                    />
                                                                    {errorsTool.toolContents?.[index]?.slug && <span className="text-wrap text-red-600 text-sm">{errorsTool.toolContents[index]?.slug?.message}</span>}
                                                                </div>
                                                            </Field>
                                                        )}
                                                    />
                                                    <Controller
                                                        name={`toolContents.${index}.languageId`}
                                                        control={control}
                                                        render={({ field }) => (
                                                            <Field className="grid gap-2">
                                                                <div className="flex flex-col gap-2">
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
                                                                    {errorsTool.toolContents?.[index]?.languageId && <span className="text-wrap text-red-600 text-sm">{errorsTool.toolContents[index]?.languageId?.message}</span>}
                                                                </div>

                                                            </Field>
                                                        )}
                                                    />
                                                </div>
                                                <Controller
                                                    name={`toolContents.${index}.description`}
                                                    control={control}
                                                    render={({ field }) => (
                                                        <div className="grid gap-2">
                                                            <div className="flex flex-col gap-2">
                                                                <Label htmlFor="description">Description</Label>
                                                                <Input
                                                                    {...field}
                                                                    placeholder="Informe a descrição..."
                                                                />
                                                                {errorsTool.toolContents?.[index]?.description && <span className="text-wrap text-red-600 text-sm">{errorsTool.toolContents[index]?.description?.message}</span>}
                                                            </div>
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
                                                                    onDragOver={(e) => e.preventDefault()}
                                                                    onDrop={(e) => handleDrop(e, field, index)}
                                                                />
                                                            </InputGroup>
                                                            {errorsTool.toolContents?.[index]?.content && <span className="text-wrap text-red-600 text-sm">{errorsTool.toolContents[index]?.content?.message}</span>}
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