"use client";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Field } from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Select, SelectContent, SelectGroup, SelectItem, SelectLabel, SelectTrigger, SelectValue } from "@/components/ui/select";
import { certificateSchema, CertificateSchema } from "@/domain/schemas/CertificateSchema";
import { MediaSchema } from "@/domain/schemas/MediaSchema";
import { useCreateCertificate } from "@/hooks/Certificate/useCreateCertificate";
import { useGetByIdCertificate } from "@/hooks/Certificate/useGetByIdCertificate";
import { useUpdateCertificate } from "@/hooks/Certificate/useUpdateCertificate";
import { addMediaService } from "@/services/client/media-services";
import { zodResolver } from "@hookform/resolvers/zod";
import { ImageIcon } from "lucide-react";
import { useSearchParams } from "next/navigation";
import { useEffect, useState } from "react";
import { Controller, ControllerRenderProps, useForm } from "react-hook-form";
import { toast } from "sonner";

export default function CertificateCreate() {
    const searchParams = useSearchParams();
    const certificateId = searchParams.get("certificateId") || undefined;
    const { data: certificate } = useGetByIdCertificate(certificateId);
    const { mutateAsync: createCertificate } = useCreateCertificate();
    const { mutateAsync: updateCertificate } = useUpdateCertificate();
    const [certificatePreview, setCertificatePreview] = useState<string | null>(null);
    const { control, handleSubmit, formState: { errors }, watch, reset, getValues, setValue } = useForm<CertificateSchema>({
        resolver: zodResolver(certificateSchema),
        defaultValues: {
            status: "DRAFT",
            media: undefined
        }
    })
    useEffect(() => {
        if (!certificate) return;
        reset({
            ...certificate
        })
    }, [certificate, reset])
    const onSubmit = async (data: CertificateSchema) => {
        console.log(data);
        if(certificate){
            if(certificate.id != null)
                await updateCertificate({id:certificate.id, data:data });
        }else{
            await createCertificate(data);
        }
    }
    const handleImage = async (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0];
        if (!file) return;
        const response = await addMediaService({
            file: file,
            ownerType: "Certificate"
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
        setCertificatePreview(`${process.env.NEXT_PUBLIC_FILES_URL}/${media.url}`);
    }
    return (
        <>
            <main className="relative mx-auto flex min-h-full inset-0 w-full max-w-[1440px] justify-center">
                <section className="relative w-full min-h-screen px-10 py-20 flex flex-col">
                    <div className="flex flex-col gap-3 sm:flex-row py-10 md:p-10 sm:items-center justify-between">
                        <h1 className="text-3xl md:text-5xl font-semibold">{certificate ? `Update Certificate` : `Create Certificate`}</h1>
                    </div>
                    <div className="flex md:p-10">
                        <form onSubmit={handleSubmit(onSubmit,
                            (errors) => {
                                console.log("Erros RHF:");
                                console.dir(errors, { depth: null });
                            })} className="flex-1 flex flex-col gap-2 min-h-0">
                            <Card className="">
                                <CardHeader className="flex flex-col md:flex-row md:items-center justify-between">
                                    <CardTitle>{certificate ? `Update Certificate` : `Write Certificate`}</CardTitle>
                                    <div className="flex gap-2">
                                        <Controller
                                            name="status"
                                            control={control}
                                            render={({ field }) => (
                                                <Select
                                                    onValueChange={field.onChange}
                                                    value={field.value}
                                                >
                                                    <SelectTrigger className="w-full">
                                                        <SelectValue placeholder="Selecione o status" />
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
                                                {certificatePreview ? (
                                                    <>
                                                        <div className="relative">
                                                            <img
                                                                src={certificatePreview}
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
                                        <div className="grid gap-2">
                                            <div className="flex flex-col gap-2">
                                                <Label htmlFor="imgFile">Imagem</Label>
                                                <Input
                                                    id="imgFile"
                                                    type="file"
                                                    className="cursor-pointer"
                                                    onChange={handleImage}
                                                />
                                            </div>
                                            {errors.media && <span className="text-wrap text-red-600 text-sm">{errors.media.message}</span>}
                                        </div>
                                    </div>
                                    <div className="py-2">
                                        <Controller
                                            name={`title`}
                                            control={control}
                                            render={({ field }) => (
                                                <Field className="grid gap-2">
                                                    <div className="flex flex-col gap-2">
                                                        <Label htmlFor="title">Title</Label>
                                                        <Input
                                                            {...field}
                                                            placeholder="Informe o titulo..."
                                                        />
                                                    </div>
                                                    {errors.title && <span className="text-wrap text-red-600 text-sm">{errors.title.message}</span>}
                                                </Field>
                                            )}
                                        />
                                    </div>
                                    <div className="py-2">
                                        <Controller
                                            name={`description`}
                                            control={control}
                                            render={({ field }) => (
                                                <Field className="grid gap-2">
                                                    <div className="flex flex-col gap-2">
                                                        <Label htmlFor="description">Description</Label>
                                                        <Input
                                                            {...field}
                                                            placeholder="Informe a descrição..."
                                                        />
                                                    </div>
                                                    {errors.description && <span className="text-wrap text-red-600 text-sm">{errors.description.message}</span>}
                                                </Field>
                                            )}
                                        />
                                    </div>
                                    <div className="py-2">
                                        <Controller
                                            name={`institution`}
                                            control={control}
                                            render={({ field }) => (
                                                <Field className="grid gap-2">
                                                    <div className="flex flex-col gap-2">
                                                        <Label htmlFor="institution">Institution</Label>
                                                        <Input
                                                            {...field}
                                                            placeholder="Informe a instituição..."
                                                        />
                                                    </div>
                                                    {errors.institution && <span className="text-wrap text-red-600 text-sm">{errors.institution.message}</span>}
                                                </Field>
                                            )}
                                        />
                                    </div>
                                    <div className="grid grid-cols-1 md:grid-cols-2 gap-2 py-2">
                                        <Controller
                                            name={`verificationUrl`}
                                            control={control}
                                            render={({ field }) => (
                                                <Field className="grid gap-2">
                                                    <div className="flex flex-col gap-2">
                                                        <Label htmlFor="institution">Verification Url</Label>
                                                        <Input
                                                            {...field}
                                                            placeholder="Informe a instituição..."
                                                        />
                                                    </div>
                                                    {errors.verificationUrl && <span className="text-wrap text-red-600 text-sm">{errors.verificationUrl.message}</span>}
                                                </Field>
                                            )}
                                        />
                                        <Controller
                                            name={`certificateType`}
                                            control={control}
                                            render={({ field }) => (
                                                <Field className="grid gap-2">
                                                    <div className="flex flex-col gap-2">
                                                        <Label htmlFor="language">Tipo</Label>
                                                        <Select
                                                            onValueChange={(value) => field.onChange(value)}
                                                            value={field.value}
                                                        >
                                                            <SelectTrigger className="w-full ">
                                                                <SelectValue placeholder="Selecione o tipo" />
                                                            </SelectTrigger>
                                                            <SelectContent>
                                                                <SelectGroup>
                                                                    <SelectLabel>Tipo</SelectLabel>
                                                                    <SelectItem value="TechnicalCourse">Curso Técnico</SelectItem>
                                                                    <SelectItem value="HigherEducation">Ensino Superior</SelectItem>
                                                                    <SelectItem value="ProfessionalCourse">Curso Profissionalizante</SelectItem>
                                                                    <SelectItem value="Bootcamp">Bootcamp</SelectItem>
                                                                    <SelectItem value="Certification">Certificação</SelectItem>
                                                                    <SelectItem value="Workshop">Oficina</SelectItem>
                                                                    <SelectItem value="Seminar">Seminário</SelectItem>
                                                                    <SelectItem value="ExtensionCourse">Curso Extensivo</SelectItem>
                                                                </SelectGroup>
                                                            </SelectContent>
                                                        </Select>
                                                    </div>
                                                    {errors.certificateType && <span className="text-wrap text-red-600 text-sm">{errors.certificateType.message}</span>}
                                                </Field>
                                            )}
                                        />
                                    </div>
                                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-2 py-2">
                                        <Controller
                                            name={`credentialId`}
                                            control={control}
                                            render={({ field }) => (
                                                <Field className="grid gap-2">
                                                    <div className="flex flex-col gap-2">
                                                        <Label htmlFor="credential">Credential</Label>
                                                        <Input
                                                            {...field}
                                                            placeholder="Informe a instituição..."
                                                        />
                                                    </div>
                                                    {errors.credentialId && <span className="text-wrap text-red-600 text-sm">{errors.credentialId.message}</span>}
                                                </Field>
                                            )}
                                        />
                                        <Controller
                                            name={`issuerDate`}
                                            control={control}
                                            render={({ field }) => (
                                                <Field className="grid gap-2">
                                                    <div className="flex flex-col gap-2">
                                                        <Label htmlFor="credential">Issuer Date *</Label>
                                                        <Input
                                                            value={
                                                                field.value
                                                                    ? field.value.toISOString().split("T")[0]
                                                                    : ""
                                                            }
                                                            onChange={(event) => {
                                                                field.onChange(
                                                                    event.target.value
                                                                        ? new Date(event.target.value)
                                                                        : null
                                                                );
                                                            }}
                                                            name={field.name}
                                                            ref={field.ref}
                                                            onBlur={field.onBlur}
                                                            type="date"
                                                            placeholder="Informe a instituição..."
                                                        />
                                                    </div>
                                                    {errors.credentialId && <span className="text-wrap text-red-600 text-sm">{errors.credentialId.message}</span>}
                                                </Field>
                                            )}
                                        />
                                        <Controller
                                            name={`workLoadHours`}
                                            control={control}
                                            render={({ field }) => (
                                                <Field className="grid gap-2">
                                                    <div className="flex flex-col gap-2">
                                                        <Label htmlFor="institution">Horas trabalhadas</Label>
                                                        <Input
                                                            {...field}
                                                            type="number"
                                                            placeholder="Informe as horas..."
                                                        />
                                                    </div>
                                                    {errors.workLoadHours && <span className="text-wrap text-red-600 text-sm">{errors.workLoadHours.message}</span>}
                                                </Field>
                                            )}
                                        />
                                    </div>
                                </CardContent>
                            </Card>
                        </form>
                    </div>
                </section>
            </main>
        </>
    )
}