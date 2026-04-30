import z from "zod";
import { toolContentSchema } from "./ToolContentSchema";
import { categorySchema } from "./CategorySchema";
export const Status = z.enum(["DRAFT","PUBLISH","ARCHIVED"]);
export const toolSchema = z.object({
    id: z.string().optional(),
    imgUrl: z.instanceof(File,{message: "A imagem da ferramenta é obrigatória."})
        .refine((file) => file.size <= 2_000_000,"A imagem deve ter no máximo 2 MB.")
        .refine((file) => ["image/jpeg", "image/png", "image/webp"].includes(file.type), "Formato de imagem inválido. Use JPEG, PNG ou WEBP."),
    toolContents: z.array(toolContentSchema).min(1,"É obrigatório ter pelo menos um conteúdo sobre a ferramenta."),
    categories: z.array(categorySchema).min(1,"É obrigatório ter pelo menos uma categoria relacionada a ferramenta."),
    status: Status
})
export const toolViewSchema = z.object({
    id: z.string(),
    imgUrl: z.string(),
    toolContents: z.array(toolContentSchema),
    categories: z.array(categorySchema),
    status: Status
})
export type ToolSchema = z.infer<typeof toolSchema>

