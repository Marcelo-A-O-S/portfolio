"use client"
import { Button } from "@/components/ui/button";
import { Card, CardAction, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Field, FieldDescription, FieldLabel } from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import { InputGroup, InputGroupAddon, InputGroupButton, InputGroupText, InputGroupTextarea } from "@/components/ui/input-group";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { postSchema, PostSchema } from "@/domain/schemas/PostSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import { FileCodeIcon, CopyIcon } from "lucide-react";
import { useState } from "react";
import { Controller, useFieldArray, useForm } from "react-hook-form";
import { Switch } from "@/components/ui/switch"
import MarkdownRenderer from "@/components/markdown-renderer";
import { addPostService } from "@/services/client/post-services";
import { toast } from "sonner";
import { useCategories } from "@/hooks/useCategories";
import { useLanguages } from "@/hooks/useLanguages";
import { useSearchParams } from "next/navigation";
import { SelectTrigger, SelectValue, SelectContent, SelectGroup, SelectLabel, SelectItem, Select } from "@/components/ui/select";
export default function ProjectCreate() {
    const searchParams = useSearchParams();
    const toolId = searchParams.get("postId") || undefined;
    const { data: categories } = useCategories();
    const { data: languages } = useLanguages();
    const [preview, openPreview] = useState(false);
    const [postData, setPostData] = useState({})
    const [open, setOpen] = useState(false);
    const { control, handleSubmit, formState: { }, watch } = useForm<PostSchema>({
        resolver: zodResolver(postSchema),
        defaultValues: {
            imgUrl: '',
            status: "DRAFT",
            postContents: [{
                content: '',
                description: '',
                title: '',
                slug: '',
            }]
        }
    });
    const { fields: fieldPostContents, append } = useFieldArray({
        control,
        name: "postContents"
    })
    const { fields: fieldTools, append: appendFieldTools, remove: removeFieldTool } = useFieldArray({
        control,
        name: "tools"
    })
    const { fields: fieldCategories, append: appendFieldCategory, remove: removeFieldCategory } = useFieldArray({
        control,
        name: "categories"
    })
    const categoriesWatch = watch("categories");
    const toolsWatch = watch("tools");
    const onSubmit = async (data: PostSchema) => {
        const response = await addPostService(data);
        if (response.status !== 200 && response.status !== 201) {
            return toast.error("Erro ao salvar postagem: ", response.data.message);
        }
    }
    return (
        <>
            <main className="relative mx-auto flex min-h-full inset-0 w-screen max-w-[1440px] justify-center bg-background overflow-x-hidden">
                <section className="relative w-full min-h-screen px-10 py-20 flex flex-col">
                    <div className="flex flex-col gap-3 sm:flex-row py-10 md:p-10 sm:items-center justify-between">
                        <h1 className="text-3xl md:text-5xl font-semibold">Create Post</h1>
                        <div className="flex gap-2 items-center">
                            <Button type="button"
                                className="cursor-pointer"
                                onClick={() =>
                                    append({
                                        languageId: "",
                                        title: "",
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
                                <CardContent className="flex-1 flex flex-col min-h-0">
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
                                    <div className="flex flex-col gap-2  pb-3 w-full">
                                        <Label>Tools</Label>
                                        <Button
                                            className="cursor-pointer"
                                            type="button"
                                            variant="outline"
                                            onClick={() => setOpen(true)}
                                        >
                                            + Adicionar ferramenta
                                        </Button>
                                        <div className="flex flex-wrap gap-2">
                                            {toolsWatch?.map((tool, index) => (
                                                <div
                                                    key={tool.id}
                                                    className="flex items-center gap-2 px-3 py-1 bg-muted rounded-full text-sm"
                                                >
                                                    {tool.toolContents.map((tc, index) => (
                                                        <span key={index}>{`${tc.name}`}</span>
                                                    ))}
                                                    <button type="button" className="cursor-pointer" onClick={() => removeFieldTool(index)}>
                                                        ✕
                                                    </button>
                                                </div>
                                            ))}
                                        </div>
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
                                    </div>
                                    {fieldPostContents.map((item, index) => (
                                        <div key={item.id} className="w-full max-h-full h-[500px] flex flex-col overflow-hidden">
                                            <div className="flex flex-col gap-6 flex-1 min-h-0 border-t pb-3 py-3">
                                                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 md:gap-2">
                                                    <Controller
                                                        name={`postContents.${index}.title`}
                                                        control={control}
                                                        render={({ field }) => (
                                                            <Field className="grid gap-2">
                                                                <Label htmlFor="title">Title</Label>
                                                                <Input
                                                                    {...field}
                                                                    placeholder="Informe o titulo..."
                                                                />
                                                            </Field>
                                                        )}
                                                    />
                                                    <Controller
                                                        name={`postContents.${index}.slug`}
                                                        control={control}
                                                        render={({ field }) => (
                                                            <Field className="grid gap-2">
                                                                <Label htmlFor="slug">Slug</Label>
                                                                <Input
                                                                    {...field}
                                                                    placeholder="Informe a URL... "
                                                                />
                                                            </Field>
                                                        )}
                                                    />
                                                    <Controller
                                                        name={`postContents.${index}.languageId`}
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
                                                    name={`postContents.${index}.description`}
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
                                                    name={`postContents.${index}.content`}
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
                                                        onClick={() => removeFieldTool(index)}
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
            </main>
        </>
    )
}