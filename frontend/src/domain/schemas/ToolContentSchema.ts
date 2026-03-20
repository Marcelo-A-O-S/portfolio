import z from "zod";
import { languageSchema } from "./LanguageSchema";
export const toolContentSchema = z.object({
    id: z.uuid().optional(),
    toolId: z.uuid().optional(),
    name: z.string().nonempty(),
    description: z.string().nonempty(),
    content: z.string().nonempty(),
    slug: z.string().nonempty(),
    languageId: z.uuid().optional(),
    language: languageSchema.optional()
})
export type ToolContentSchema = z.infer<typeof toolContentSchema>