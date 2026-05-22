import z from "zod";
import { languageSchema } from "./LanguageSchema";
export const postContentSchema = z.object({
    id: z.uuid().optional(),
    postId: z.uuid().optional(),
    languageId: z.uuid("Idioma inválido.").optional(),
    language: languageSchema.optional(),
    title: z.string().min(2).max(50).nonempty("O titulo é obrigatório."),
    description: z.string().min(2).max(255).nonempty("A descrição é obrigatória."),
    content: z.string().nonempty("O conteúdo é obrigatório."),
    imagesUrls: z.array(z.string()).optional(),
    slug: z.string().nonempty("O slug é obrigatório."),
})
export type PostContentSchema = z.infer<typeof postContentSchema>;