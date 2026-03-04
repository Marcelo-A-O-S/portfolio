import z from "zod";
import { categorySchema } from "./CategorySchema";
import { toolSchema } from "./ToolSchema";
export const PostStatusEnum = z.enum(["DRAFT", "PUBLISH"])
export const postSchema = z.object({
    certificateId: z.string().optional(),
    title: z.string().nonempty(""),
    description: z.string().nonempty(),
    content: z.string().nonempty(),
    imageUrl: z.string().nonempty(),
    status: PostStatusEnum,
    slug: z.string().nonempty(),
    categories: z.array(categorySchema),
    tools: z.array(toolSchema)
})
export type PostSchema = z.infer<typeof postSchema>