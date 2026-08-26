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
    title: z.string().nonempty(),
    description: z.string().nonempty(),
    credentialId: z.string().optional(),
    verificationUrl: z.string().optional(),
    institution: z.string().nonempty(),
    workLoadHours: z.number().optional(),
    status: Status,
    certificateType: CertificateType,
    createdAt: z.date(),
    updatedAt: z.date().optional(),
    issuerDate: z.date()
})
export type CertificateSchema = z.infer<typeof certificateSchema>;