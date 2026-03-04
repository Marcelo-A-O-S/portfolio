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
import { Controller, useForm } from "react-hook-form";
import { Switch } from "@/components/ui/switch"
import MarkdownRenderer from "@/components/markdown-renderer";
import { CategorySchema, categorySchema } from "@/domain/schemas/CategorySchema";
export default function CategoryCreatePage() {
    const { control, handleSubmit, formState: { }, watch } = useForm<CategorySchema>({
        resolver: zodResolver(categorySchema),
        defaultValues: {
            name: ''
        }
    });
    const onSubmit = async (data: CategorySchema) => {
    }
    return (
        <>
            <main className="relative mx-auto flex min-h-full inset-0 w-full max-w-[1440px] justify-center bg-background overflow-x-hidden">
                <section className="relative w-screen h-full px-10 py-20 flex flex-col items-center">
                    <div className="w-full flex p-10 items-center justify-between">
                        <h1 className="text-3xl md:text-5xl font-semibold">Create Tool</h1>
                    </div>
                    <div className="flex gap-4">
                        <Card className="w-[80vw] md:w-[80vw] lg:w-[40vw] max-h-full h-full flex flex-col overflow-hidden">
                            <CardHeader>
                                <CardTitle>Write Post</CardTitle>
                            </CardHeader>
                            <CardContent className="flex-1 flex flex-col min-h-0">
                                <form onSubmit={handleSubmit(onSubmit)} className="flex-1 flex flex-col min-h-0">
                                    <div className="flex flex-col gap-6 flex-1 min-h-0">
                                        <Controller
                                            name="name"
                                            control={control}
                                            render={({ field }) => (
                                                <div className="grid gap-2">
                                                    <Label htmlFor="title">Name</Label>
                                                    <Input
                                                        {...field}
                                                        placeholder="Informe o nome da categoria..."
                                                    />
                                                </div>
                                            )}
                                        />
                                    </div>
                                </form>
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