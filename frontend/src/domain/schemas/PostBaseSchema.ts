import z from "zod";
import { categorySchema } from "./CategorySchema";
import { mediaSchema } from "./MediaSchema";
import { authorSchema } from "./AuthorSchema";
import { linkSchema } from "./LinkSchema";
export const Status = z.enum(["DRAFT", "PUBLISH", "ARCHIVED"]);
export const linkInPostBaseSchema = linkSchema.omit({
    toolId: true,
    postId: true
})
export const postBaseSchema = z.object({
    id: z.uuid().optional(),
    mediaId: z.uuid().optional(),
    media: mediaSchema.optional(),
    categories: z.array(categorySchema).min(1, "É obrigatório ter pelo menos uma categoria relacionada ao projeto."),
    likes: z.number(),
    liked: z.boolean(),
    comments: z.number(),
    status: Status,
    links: z.array(linkInPostBaseSchema).optional(),
    author: authorSchema.optional(),
    createdAt: z.string().optional(),
    updatedAt: z.string().optional()
})