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
import { Category } from "@/domain/types/Category";
import { CategorySchema } from "@/domain/schemas/CategorySchema";
export default function ToolCreatePage() {
    const [preview, openPreview] = useState(false);
    const [previewIndex, setPreviewIndex] = useState<number | null>(null);
    const [open, setOpen] = useState(false);
    const [selectedCategories, setSelectedCategories] = useState<CategorySchema[]>([])
    const { data: categories } = useCategories();
    const { data: languages } = useLanguages();
    const { control, handleSubmit, formState: { }, watch } = useForm<ToolSchema>({
        resolver: zodResolver(toolSchema),
        defaultValues: {
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
    const { fields: fieldToolContents, append, remove } = useFieldArray({
        control,
        name: "toolContents"
    });
    const { fields: fieldCategories } = useFieldArray({
        control,
        name: "categories"
    })
    console.log(categories);
    const onSubmit = async (data: ToolSchema) => {
        console.log(data);
    }
    const addCategory = (data: CategorySchema) => {

    }
    const removeCategory = (id: string) => {

    }
    const contents = watch("toolContents")
    return (
        <>
            <Dialog open={open} onOpenChange={setOpen}>
                <DialogContent>
                    <DialogHeader>
                        <DialogTitle>Selecionar categorias</DialogTitle>
                    </DialogHeader>

                    {/* BUSCA */}
                    <Input placeholder="Buscar categoria..." />

                    {/* LISTA */}
                    <div className="max-h-60 overflow-y-auto">
                        {categories?.map((cat, index) => (
                            <div
                                key={cat.id}
                                className="flex items-center justify-between p-2 hover:bg-muted rounded"
                            >
                                <span>{cat.categoryContents.find(cc => cc.languageId == contents[index].languageId)?.name}</span>

                                <Button
                                    size="sm"
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
                    <div className="flex flex-col gap-3 sm:flex-row  py-10 md:p-10 sm:items-center justify-between">
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
                            {fieldToolContents.map((item, index) => (
                                <div key={item.id} className="flex gap-4">
                                    <Card className="w-full max-h-full h-[600px] flex flex-col overflow-hidden">
                                        <CardHeader className="flex flex-row items-center border-b pb-3 justify-between">
                                            <CardTitle>Write Post</CardTitle>
                                            <div className="flex gap-1">
                                                <Button
                                                    type="button"
                                                    variant="destructive"
                                                    onClick={() => remove(index)}
                                                >
                                                    Remove
                                                </Button>
                                                <Button
                                                    type="button"
                                                    onClick={() =>
                                                        setPreviewIndex(previewIndex === index ? null : index)
                                                    }
                                                >
                                                    Preview
                                                </Button>
                                            </div>
                                        </CardHeader>
                                        <CardContent className="flex-1 flex flex-col min-h-0">
                                            {previewIndex === index ?
                                                (
                                                    <MarkdownRenderer content={contents[index]?.content || "# Crie agora uma postagem! \n\nDescreva sua **postagem** no campo content ..."} />
                                                ) :
                                                (<div className="flex flex-col gap-6 flex-1 min-h-0">
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
                                                                    {field.value && (
                                                                        <span className="text-xs text-muted-foreground">
                                                                            Idioma selecionado
                                                                        </span>
                                                                    )}
                                                                </Field>
                                                            )}
                                                        />
                                                    </div>
                                                    <div className="flex flex-col gap-2">
                                                        <Label>Categorias</Label>

                                                        {/* BOTÃO DE ADICIONAR */}
                                                        <Button
                                                            type="button"
                                                            variant="outline"
                                                            onClick={() => setOpen(true)}
                                                        >
                                                            + Adicionar categoria
                                                        </Button>

                                                        {/* TAGS */}
                                                        <div className="flex flex-wrap gap-2">
                                                            {selectedCategories.map((cat) => (
                                                                <div
                                                                    key={cat.id}
                                                                    className="flex items-center gap-2 px-3 py-1 bg-muted rounded-full text-sm"
                                                                >
                                                                    {cat.categoryContents.find(cc => cc.languageId == contents[index].languageId)?.name}
                                                                    <button onClick={() => cat.id? removeCategory(cat.id) : console.log("Identificador vazio")}>
                                                                        ✕
                                                                    </button>
                                                                </div>
                                                            ))}
                                                        </div>
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
                                                            <Field className="flex flex-col flex-1  min-h-0">
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
                                                    <Field orientation="horizontal" className="w-fit">
                                                        <FieldLabel htmlFor="2fa">Publish</FieldLabel>
                                                        <Switch id="2fa" />
                                                    </Field>
                                                </div>)}

                                        </CardContent>
                                        <CardFooter className="flex-col gap-2">
                                        </CardFooter>
                                    </Card>
                                </div>
                            ))}
                        </form>
                    </div>
                </section>
            </main >
        </>
    )
}