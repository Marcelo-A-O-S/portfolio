import z from "zod";
import { languageSchema } from "./LanguageSchema";
export const languageProjectionSchema = languageSchema.omit({
    createdAt: true,
    updatedAt: true
})
export const certificateContentSchema = z.object({
    id: z.uuid().optional(),
    languageId: z.uuid(),
    languageProjection: languageProjectionSchema.optional(),
    title: z.string("O titulo é obrigatório.").nonempty("O titulo é obrigatório."),
    description: z.string("A descrição é obrigatória.").nonempty("A descrição é obrigatória.")
})
export type CertificateContentSchema = z.infer<typeof certificateContentSchema>;
