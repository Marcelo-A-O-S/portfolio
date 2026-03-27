import z from "zod";
import { categorySchema } from "./CategorySchema";
import { toolSchema } from "./ToolSchema";
import { postContentSchema } from "./PostContentSchema";
export const PostStatusEnum = z.enum(["DRAFT", "PUBLISH"])
export const postSchema = z.object({
    id: z.uuid().optional(),
    imgUrl: z.string().nonempty(),
    status: PostStatusEnum,
    postContents: z.array(postContentSchema),
    categories: z.array(categorySchema),
    tools: z.array(toolSchema)
})
export type PostSchema = z.infer<typeof postSchema>