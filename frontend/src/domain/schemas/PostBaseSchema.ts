import z from "zod";
import { categorySchema } from "./CategorySchema";
import { mediaSchema } from "./MediaSchema";
export const Status = z.enum(["DRAFT", "PUBLISH", "ARCHIVED"]);
export const postBaseSchema = z.object({
    id: z.uuid().optional(),
    mediaId: z.uuid().optional(),
    media: mediaSchema.optional(),
    categories: z.array(categorySchema).min(1, "É obrigatório ter pelo menos uma categoria relacionada ao projeto."),
    likes: z.number(),
    liked: z.boolean(),
    comments: z.number(),
    status: Status
})