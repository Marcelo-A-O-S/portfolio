import z from "zod";
import { categorySchema } from "./CategorySchema";

export const postBaseSchema = z.object({
    id: z.uuid().optional(),
    imgUrl: z.string(),
    imgFile: z.instanceof(File, { message: "A imagem da ferramenta é obrigatória." })
        .refine((file) => file.size <= 2_000_000, "A imagem deve ter no máximo 2 MB.")
        .refine((file) => ["image/jpeg", "image/png", "image/webp"].includes(file.type), "Formato de imagem inválido. Use JPEG, PNG ou WEBP.").optional(), 
    categories: z.array(categorySchema).min(1, "É obrigatório ter pelo menos uma categoria relacionada ao projeto."),
    likes: z.number().default(0),
    liked: z.boolean().default(false),
    comments: z.number().default(0),
})