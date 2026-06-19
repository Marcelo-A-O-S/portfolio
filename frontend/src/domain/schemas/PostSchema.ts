import z from "zod";
import { toolSchema } from "./ToolSchema";
import { postContentSchema } from "./PostContentSchema";
import { postBaseSchema } from "./PostBaseSchema";
export const PostStatusEnum = z.enum(["DRAFT", "PUBLISH"])
export const toolInPostSchema = toolSchema.omit({
    categories: true
});
export const postSchema = postBaseSchema.extend({
    postContents: z.array(postContentSchema).min(1, "É obrigatório ter pelo menos um conteudo relacionado ao projeto."),
    tools: z.array(toolInPostSchema).min(1, "É obrigatório ter pelo menos uma ferramenta relacionada ao projeto."),
    status: PostStatusEnum,
})
export type PostSchema = z.infer<typeof postSchema>