import z from "zod";
import { languageSchema } from "./LanguageSchema";

export const linkDescriptionSchema = z.object({
    id: z.uuid().optional(),
    title: z.string().nonempty(),
    languageId: z.uuid(),
    language: languageSchema.optional()
})
export type LinkDescriptionSchema = z.infer<typeof linkDescriptionSchema>;