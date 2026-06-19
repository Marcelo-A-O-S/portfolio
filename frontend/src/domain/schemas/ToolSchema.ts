import z from "zod";
import { postBaseSchema, Status } from "./PostBaseSchema";
import { toolContentSchema } from "./ToolContentSchema";
export const toolSchema = postBaseSchema.extend({
    toolContents: z.array(toolContentSchema).min(1, "É obrigatório ter pelo menos um conteúdo sobre a ferramenta.")
})
export type ToolSchema = z.infer<typeof toolSchema>

