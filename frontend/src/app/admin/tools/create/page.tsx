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
export default function ToolCreatePage() {
    const [preview, openPreview] = useState(false);
    const { control, handleSubmit, formState: { }, watch } = useForm<ToolSchema>({
        resolver: zodResolver(toolSchema),
        defaultValues: {
            toolContents: [
                {
                    content: "",
                    description: "",
                    name: "",
                    slug: "",
                    language: ""
                }
            ],

        }
    });
    const { fields: fieldToolContents, append } = useFieldArray({
        control,
        name: "toolContents"
    });
    const onSubmit = async (data: ToolSchema) => {
    }
    const contents = watch("toolContents")
    return (
        <>
            <main className="relative mx-auto flex min-h-full inset-0 w-screen max-w-[1440px] justify-center bg-background overflow-x-hidden">
                <section className="relative w-full min-h-screen px-10 py-20 flex flex-col">
                    <div className="flex flex-col gap-3 sm:flex-row  py-10 md:p-10 sm:items-center justify-between">
                        <h1 className="text-3xl md:text-5xl font-semibold">Create Tool</h1>
                        <div className="flex gap-2 items-center">
                            <Button type="button"
                                className="cursor-pointer"
                                onClick={() =>
                                    append({
                                        language: "",
                                        name: "",
                                        slug: "",
                                        content: "",
                                        description: ""
                                    })
                                } >Add translation</Button>

                        </div>
                    </div>
                    <div className="flex md:p-10">
                        <form onSubmit={handleSubmit(onSubmit)} className="flex-1 flex flex-col min-h-0">
                            {fieldToolContents.map((item, index) => (
                                <div key={item.id} className="flex gap-4">
                                    <Card className="w-full max-h-full h-[600px] flex flex-col overflow-hidden">
                                        <CardHeader className="flex items-center justify-between">
                                            <CardTitle>Write Post</CardTitle>
                                            <div>
                                                <Button type="button" className="cursor-pointer" onClick={() => openPreview(!preview)} >Preview</Button>
                                            </div>
                                        </CardHeader>
                                        <CardContent className="flex-1 flex flex-col min-h-0">
                                            {preview ?
                                                (
                                                    <MarkdownRenderer content={contents[index]?.content || "# Crie agora uma postagem! \n\nDescreva sua **postagem** no campo content ..."} />
                                                ) :
                                                (<div className="flex flex-col gap-6 flex-1 min-h-0">
                                                    <div className="grid grid-cols-1 md:grid-cols-2 md:gap-2">
                                                        <Controller
                                                            name={`toolContents.${index}.name`}
                                                            control={control}
                                                            render={({ field }) => (
                                                                <div className="grid gap-2">
                                                                    <Label htmlFor="title">Name</Label>
                                                                    <Input
                                                                        {...field}
                                                                        placeholder="Informe o nome da ferramenta..."
                                                                    />
                                                                </div>
                                                            )}
                                                        />
                                                        <Controller
                                                            name={`toolContents.${index}.slug`}
                                                            control={control}
                                                            render={({ field }) => (
                                                                <div className="grid gap-2">
                                                                    <Label htmlFor="title">Slug</Label>
                                                                    <Input
                                                                        {...field}
                                                                        placeholder="Informe um nome amigável da URL... "
                                                                    />
                                                                </div>
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
                                                                    placeholder="Informe uma breve descrição da ferramenta..."
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