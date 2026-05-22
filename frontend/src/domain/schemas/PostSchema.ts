import z from "zod";
import { categorySchema } from "./CategorySchema";
import { toolSchema } from "./ToolSchema";
import { postContentSchema } from "./PostContentSchema";
export const PostStatusEnum = z.enum(["DRAFT", "PUBLISH"])
export const toolInPostSchema = toolSchema.omit({
    imgFile: true,
    categories: true
});
export const postSchema = z.object({
    id: z.uuid().optional(),
    imgUrl: z.string(),
    imgFile: z.instanceof(File, { message: "A imagem da ferramenta é obrigatória." })
        .refine((file) => file.size <= 2_000_000, "A imagem deve ter no máximo 2 MB.")
        .refine((file) => ["image/jpeg", "image/png", "image/webp"].includes(file.type), "Formato de imagem inválido. Use JPEG, PNG ou WEBP.").optional(), 
    postContents: z.array(postContentSchema).min(1, "É obrigatório ter pelo menos um conteúdo sobre o projeto."),
    categories: z.array(categorySchema).min(1, "É obrigatório ter pelo menos uma categoria relacionada ao projeto."),
    tools: z.array(toolInPostSchema).min(1, "É obrigatório ter pelo menos uma ferramenta relacionada ao projeto."),
    status: PostStatusEnum
})
export type PostSchema = z.infer<typeof postSchema>