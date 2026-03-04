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
import { Controller, useForm } from "react-hook-form";
import { Switch } from "@/components/ui/switch"
import MarkdownRenderer from "@/components/markdown-renderer";
import { addPostService } from "@/services/client/post-services";
import { toast } from "sonner";
export default function ProjectCreate() {
    const [preview, openPreview] = useState(false);
    const [postData, setPostData] = useState({})
    const { control, handleSubmit, formState: { }, watch } = useForm<PostSchema>({
        resolver: zodResolver(postSchema),
        defaultValues: {
            content: '',
            description: '',
            title: '',
            imageUrl: '',
            status: "DRAFT"
        }
    });
    const onSubmit = async (data: PostSchema) => {
        const response = await addPostService(data);
        if(response.status !== 200 && response.status !== 201){
            return toast.error("Erro ao salvar postagem: ", response.data.message);
        }
    }
    const contentValue = watch("content");
    return (
        <>
            <main className="relative mx-auto flex min-h-full inset-0 w-screen max-w-[1440px] justify-center bg-background overflow-x-hidden">
                <section className="relative w-screen h-full px-10 py-20 flex flex-col items-center">
                    <div className="w-full flex p-10 items-center justify-between">
                        <h1 className="text-3xl md:text-5xl font-semibold">Create Post</h1>
                        <div className="flex gap-2 items-center">
                            <Button className="cursor-pointer" onClick={() => openPreview(!preview)} >Preview</Button>
                        </div>
                    </div>
                    <div className="flex gap-4 ">
                        <Card className="w-[40vw] max-h-full h-[600px] flex flex-col overflow-hidden">
                            <CardHeader>
                                <CardTitle>Write Post</CardTitle>
                            </CardHeader>
                            <CardContent className="flex-1 flex flex-col min-h-0">
                                <form onSubmit={handleSubmit(onSubmit)} className="flex-1 flex flex-col min-h-0">
                                    <div className="flex flex-col gap-6 flex-1 min-h-0">
                                        <Controller
                                            name="title"
                                            control={control}
                                            render={({ field }) => (
                                                <div className="grid gap-2">
                                                    <Label htmlFor="title">Title</Label>
                                                    <Input
                                                        {...field}
                                                        placeholder=""
                                                    />
                                                </div>
                                            )}
                                        />
                                        <Controller
                                            name="imageUrl"
                                            control={control}
                                            render={({ field }) => (
                                                <div className="grid gap-2">
                                                    <Label htmlFor="title">Image url</Label>
                                                    <Input
                                                        {...field}
                                                        placeholder=""
                                                    />
                                                </div>
                                            )}
                                        />
                                        <Controller
                                            name="description"
                                            control={control}
                                            render={({ field }) => (
                                                <div className="grid gap-2">
                                                    <Label htmlFor="description">Description</Label>
                                                    <Input
                                                        {...field}
                                                        placeholder=""
                                                    />
                                                </div>
                                            )}
                                        />
                                        <Controller
                                            name="content"
                                            control={control}
                                            render={({ field }) => (
                                                <Field className="flex flex-col flex-1  min-h-0">
                                                    <FieldLabel htmlFor="block-start-textarea">Content</FieldLabel>
                                                    <InputGroup className="flex-1 min-h-0">
                                                        <InputGroupTextarea
                                                            {...field}
                                                            placeholder=""
                                                            className="flex-1 resize-none overflow-y-auto"
                                                        />
                                                    </InputGroup>
                                                </Field>
                                            )}
                                        />
                                        <Field orientation="horizontal" className="w-fit">
                                            <FieldLabel htmlFor="2fa">Publish</FieldLabel>
                                            <Switch id="2fa" />
                                        </Field>
                                    </div>
                                </form>
                            </CardContent>
                            <CardFooter className="flex-col gap-2">
                            </CardFooter>
                        </Card>
                        <Card className={`${preview ? "flex" : "hidden"} `}>
                            <CardHeader>
                                <CardTitle>Preview</CardTitle>
                            </CardHeader>
                            <CardContent>
                                <MarkdownRenderer content={contentValue || "# Crie agora uma postagem! \n\nDescreva sua **postagem** no campo content ..."} />
                            </CardContent>
                            <CardFooter className="flex-col gap-2">
                            </CardFooter>
                        </Card>
                    </div>

                </section>
            </main>
        </>
    )
}