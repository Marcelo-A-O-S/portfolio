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
    const { fields: fieldPostContents , append } = useFieldArray({
        control,
        name:"postContents"
    })
    const { fields: fieldTools, append: appendFieldTools } = useFieldArray({
        control,
        name: "tools"
    })
    const { fields: fieldCategories, append: appendFieldCategory } = useFieldArray({
        control,
        name: "categories"
    })
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

                                </CardContent>
                                <CardFooter className="flex-col gap-2">
                                </CardFooter>
                            </Card>
                        </form>
                    </div>

                </section>
            </main>
        </>
    )
}