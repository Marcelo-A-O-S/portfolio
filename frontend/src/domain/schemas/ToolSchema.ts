import z from "zod";
import { toolContentSchema } from "./ToolContentSchema";
import { categorySchema } from "./CategorySchema";
export const toolSchema = z.object({
    id: z.string().optional(),
    imgUrl: z.string(),
    toolContents: z.array(toolContentSchema),
    categories: z.array(categorySchema)
})
export type ToolSchema = z.infer<typeof toolSchema>

