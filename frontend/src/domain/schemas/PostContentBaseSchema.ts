import z from "zod";
import { languageSchema } from "./LanguageSchema";

export const postContentBaseSchema = z.object({
    id: z.uuid().optional(),
    title: z.string().min(2).max(50).nonempty("O titulo é obrigatório."),
    description: z.string().min(2).max(255).nonempty("A descrição é obrigatória."),
    content: z.string().nonempty("O conteúdo é obrigatório."),
    slug: z.string().nonempty("O slug é obrigatório."),
    languageId: z.uuid("Idioma inválido.").optional(),
    language: languageSchema.optional()
})