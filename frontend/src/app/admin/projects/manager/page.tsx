"use client"
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Field, FieldLabel } from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import { InputGroup, InputGroupTextarea } from "@/components/ui/input-group";
import { Label } from "@/components/ui/label";
import { postSchema, PostSchema } from "@/domain/schemas/PostSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import { useEffect, useState } from "react";
import { Controller, ControllerRenderProps, useFieldArray, useForm } from "react-hook-form";
import { toast } from "sonner";
import { useCategories } from "@/hooks/Category/useCategories";
import { useLanguages } from "@/hooks/Language/useLanguages";
import { useSearchParams } from "next/navigation";
import { SelectTrigger, SelectValue, SelectContent, SelectGroup, SelectLabel, SelectItem, Select } from "@/components/ui/select";
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog";
import { CategorySchema } from "@/domain/schemas/CategorySchema";
import { ToolSchema } from "@/domain/schemas/ToolSchema";
import { useTools } from "@/hooks/Tool/useTools";
import { ImageIcon } from "lucide-react";
import { useGetByIdPost } from "@/hooks/Post/useGetByIdPost";
import { useCreateProject } from "@/hooks/Post/useCreateProject";
import { useUpdateProject } from "@/hooks/Post/useUpdateProject";
import { addMediaService } from "@/services/client/media-services";
import { MediaSchema } from "@/domain/schemas/MediaSchema";
export default function ProjectCreate() {
    const searchParams = useSearchParams();
    const postId = searchParams.get("postId") || undefined;
    const { data: categories } = useCategories();
    const { data: languages } = useLanguages();
    const { data: tools } = useTools();
    const { data: post } = useGetByIdPost(postId);
    const { mutateAsync: updateProject } = useUpdateProject();
    const { mutateAsync: createProject } = useCreateProject();
    const [postPreview, setPostPreview] = useState<string | null>(null);
    const [openDialogCategory, setOpenDialogCategory] = useState(false);
    const [openDialogTools, setOpenDialogTools] = useState(false);
    const { control, handleSubmit, formState: { errors: errorsPost }, watch, reset, getValues, setValue } = useForm<PostSchema>({
        resolver: zodResolver(postSchema),
        defaultValues: {
            status: "DRAFT",
            postContents: [{
                content: '',
                description: '',
                title: '',
                slug: '',
            }],
            liked: false,
            likes: 0,
            comments: 0
        }
    });
    useEffect(() => {
        if (!post) return;
        reset({
            ...post
        })
        setPostPreview(`${process.env.NEXT_PUBLIC_FILES_URL}/${post.media?.url}`);
    }, [post, reset]);
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
        console.log("Atualizando Projeto: ", data);
        if (post) {
            await updateProject({ id: post.id, data: data });
        } else {
            await createProject(data);
        }
    }
    const addTool = (data: ToolSchema) => {
        const exists = toolsWatch.some(
            (c) => c.id === data.id
        )
        if (exists) return
        appendFieldTools(data);
    }
    const addCategory = (data: CategorySchema) => {
        const exists = categoriesWatch.some(
            (c) => c.id === data.id
        )
        if (exists) return
        appendFieldCategory(data);
    }
    const handleDrop = async (e: React.DragEvent<HTMLTextAreaElement>, field: ControllerRenderProps<PostSchema, `postContents.${number}.content`>, index: number) => {
        e.preventDefault();
        const file = e.dataTransfer.files?.[0];
        if (!file || !file.type.startsWith("image/")) return;
        const textarea = e.currentTarget;
        const start = textarea.selectionStart;
        const end = textarea.selectionEnd;
        const response = await addMediaService({
            file: file,
            ownerType: "Post"
        });
        if (response.status !== 200 && response.status !== 201) {
            toast.error(`Erro ao adicionar imagem: ${response.data.message}`);
            return;
        }
        const url = response.data.url;
        const mediaId = response.data.mediaId;
        const ownerType = response.data.ownerType;
        const urlMarkdown = `\n![image](${url})\n`
        const newValue = field.value.substring(0, start) + urlMarkdown + field.value.substring(end);
        field.onChange(newValue);
        const currentImages = getValues(`postContents.${index}.images`) ?? [];
        const media: MediaSchema = {
            url: url,
            mediaId: mediaId,
            ownerType: ownerType
        };
        setValue(`postContents.${index}.images`, [...currentImages, media]);
        const imagesUpdated = getValues(`postContents.${index}.images`);
        console.log(imagesUpdated);
    }
    const handleImage = async (e: React.ChangeEvent<HTMLInputElement>, field: ControllerRenderProps<PostSchema, `media.url`>) => {
        const file = e.target.files?.[0];
        if (!file) return;
        const response = await addMediaService({
            file: file,
            ownerType: "Post"
        });
        if (response.status !== 200 && response.status !== 201) {
            toast.error(`Erro ao adicionar imagem: ${response.data.message}`);
            return;
        }
        const url = response.data.url;
        const mediaId = response.data.mediaId;
        const ownerType = response.data.ownerType;
        const media: MediaSchema = {
            url: url,
            mediaId: mediaId,
            ownerType: ownerType
        };
        setValue("mediaId", mediaId);
        setValue("media", media);
        setPostPreview(`${process.env.NEXT_PUBLIC_FILES_URL}/${media.url}`);
    }
    return (
        <>
            <Dialog open={openDialogTools} onOpenChange={setOpenDialogTools}>
                <DialogContent>
                    <DialogHeader>
                        <DialogTitle>Selecionar Ferramentas</DialogTitle>
                    </DialogHeader>
                    <Input placeholder="Buscar ferramenta..." />
                    <div className="max-h-60 overflow-y-auto">
                        {tools?.map((tool, index) => (
                            <div
                                key={tool.id}
                                className="flex items-center justify-between p-2 hover:bg-muted rounded"
                            >
                                {tool.toolContents.map((tc, index) => (
                                    <span key={index}>{`${tc.name}`}</span>
                                ))}
                                <Button
                                    size="sm"
                                    className="cursor-pointer"
                                    disabled={toolsWatch?.some(c => c.id === tool.id)}
                                    onClick={() => addTool(tool)}
                                >
                                    Adicionar
                                </Button>
                            </div>
                        ))}
                    </div>
                </DialogContent>
            </Dialog>
            <Dialog open={openDialogCategory} onOpenChange={setOpenDialogCategory}>
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
            <main className="relative mx-auto flex min-h-full inset-0 w-full max-w-[1440px] justify-center bg-background overflow-x-hidden">
                <section className="relative w-full min-h-screen px-10 py-20 flex flex-col">
                    <div className="flex flex-col gap-3 sm:flex-row py-10 md:p-10 sm:items-center justify-between">
                        <h1 className="text-3xl md:text-5xl font-semibold">Create Project</h1>
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
                        <form onSubmit={handleSubmit(onSubmit,
                                (errors) => {
                                    console.log("Erros RHF:");
                                    console.dir(errors, { depth: null });
                                })} className="flex-1 flex flex-col gap-2 min-h-0">
                            <Card className="">
                                <CardHeader className="flex flex-col md:flex-row md:items-center justify-between">
                                    <CardTitle>Write Project</CardTitle>
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
                                        <div className="flex flex-col gap-2">
                                            <div className="flex justify-between flex-col gap-2 md:flex-row">
                                                <Label>Preview</Label>
                                                <Button>Clear Preview</Button>
                                            </div>
                                            <div className="flex relative border rounded-sm h-45 items-center justify-center text-sm">
                                                {postPreview ? (
                                                    <>
                                                        <div className="relative">
                                                            <img
                                                                src={postPreview}
                                                                alt="Preview"
                                                                className="h-42 rounded border object-cover"
                                                            />
                                                        </div>
                                                    </>
                                                ) : (
                                                    <>
                                                        <div className="flex flex-col justify-center items-center">
                                                            <ImageIcon />
                                                            Imagem não adicionada
                                                        </div>
                                                    </>
                                                )}
                                            </div>
                                        </div>
                                    </div>
                                    <div className="py-2">
                                        <Controller
                                            name={`media.url`}
                                            control={control}
                                            render={({ field }) => (
                                                <div className="grid gap-2">
                                                    <div className="flex flex-col gap-2">
                                                        <Label htmlFor="imgFile">Imagem</Label>
                                                        <Input
                                                            onChange={(e) => handleImage(e, field)}
                                                            type="file"
                                                            className="cursor-pointer"
                                                        />
                                                    </div>
                                                    {errorsPost.media && <span className="text-wrap text-red-600 text-sm">{errorsPost.media.message}</span>}
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
                                            onClick={() => setOpenDialogTools(true)}
                                        >
                                            + Adicionar ferramenta
                                        </Button>
                                        <div className="flex flex-wrap gap-2">
                                            {toolsWatch?.map((tool, index) => (
                                                <div
                                                    key={tool.id}
                                                    className="flex items-center gap-2 px-3 py-1 bg-muted  text-sm"
                                                >
                                                    <div className="flex flex-col">
                                                        {tool.toolContents.map((tc, index) => (
                                                            <span key={index}>{`${tc.name}`}</span>
                                                        ))}
                                                    </div>

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
                                            onClick={() => setOpenDialogCategory(true)}
                                        >
                                            + Adicionar categoria
                                        </Button>
                                        <div className="flex flex-wrap gap-2">
                                            {categoriesWatch?.map((cat, index) => (
                                                <div
                                                    key={cat.id}
                                                    className="flex items-center gap-2 px-3 py-1 bg-muted rounded-full text-sm"
                                                >
                                                    <div className="flex flex-col">
                                                        {cat.categoryContents.map((cc, index) => (
                                                            <span key={index}>{`${cc.name}`}</span>
                                                        ))}
                                                    </div>

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
                                                                    onDragOver={(e) => e.preventDefault()}
                                                                    onDrop={(e) => handleDrop(e, field, index)}
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