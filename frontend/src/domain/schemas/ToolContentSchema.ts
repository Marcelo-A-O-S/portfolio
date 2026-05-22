import z from "zod";
import { languageSchema } from "./LanguageSchema";
export const toolContentSchema = z.object({
    id: z.uuid().optional(),
    toolId: z.uuid().optional(),
    name: z.string().min(2).max(25).nonempty("O nome é obrigatório."),
    title: z.string().min(2).max(50).nonempty("O titulo é obrigatório."),
    description: z.string().min(2).max(255).nonempty("A descrição é obrigatória."),
    content: z.string().nonempty("O conteúdo é obrigatório."),
    imagesUrls: z.array(z.string()).optional(),
    slug: z.string().nonempty("O slug é obrigatório."),
    languageId: z.uuid("Idioma inválido.").optional(),
    language: languageSchema.optional()
})
export type ToolContentSchema = z.infer<typeof toolContentSchema>