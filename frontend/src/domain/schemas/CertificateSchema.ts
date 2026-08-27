import z from "zod";
import { mediaSchema } from "./MediaSchema";
import { Status } from "./PostBaseSchema";
export const CertificateType = z.enum([
    "TechnicalCourse",
    "HigherEducation",
    "ProfessionalCourse",
    "Bootcamp",
    "Certification",
    "Workshop",
    "Seminar",
    "ExtensionCourse"
])
export const certificateSchema = z.object({
    id: z.string().optional(),
    mediaId: z.string().optional(),
    media: mediaSchema.optional(),
    title: z.string("O titulo é obrigatório.").nonempty("O titulo é obrigatório."),
    description: z.string("A descrição é obrigatória.").nonempty("A descrição é obrigatória."),
    credentialId: z.string().optional(),
    verificationUrl: z.string().optional(),
    institution: z.string("A instituição é obrigatória.").nonempty("A instituição é obrigatória."),
    workLoadHours: z.number().optional(),
    status: Status,
    certificateType: z.string("O tipo de certificado é obrigatório.")
        .min(1, "O tipo de certificado é obrigatório.")
        .nonempty("O tipo de certificado é obrigatório.")
        .refine(
            (value) => CertificateType.safeParse(value).success,
            "Tipo de certificado inválido."
        ),
    createdAt: z.date().optional(),
    updatedAt: z.date().optional(),
    issuerDate: z.date("A data do emissor é obrigatória.")
})
export type CertificateSchema = z.infer<typeof certificateSchema>;