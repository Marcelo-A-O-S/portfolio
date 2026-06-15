import z from "zod";
import { postContentBaseSchema } from "./PostContentBaseSchema";
export const toolContentSchema = postContentBaseSchema.extend({
    toolId: z.uuid().optional(),
    name: z.string().min(2).max(25).nonempty("O nome é obrigatório.")
})
export type ToolContentSchema = z.infer<typeof toolContentSchema>