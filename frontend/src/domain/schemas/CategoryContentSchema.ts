import z from "zod";
import { languageSchema, LanguageSchema } from "./LanguageSchema";
export const categoryContentSchema = z.object({
    id: z.string().uuid().optional(),
    categoryId: z.string().uuid().optional(),
    languageId: z.string().uuid().optional(),
    language: languageSchema.optional(),
    name: z.string().nonempty(),
    slug: z.string().nonempty()
})
export type CategoryContentSchema = z.infer<typeof categoryContentSchema>; 