import z from "zod";
import { toolContentSchema } from "./ToolContentSchema";
export const toolSchema = z.object({
    id: z.string().optional(),
    toolContents: z.array(toolContentSchema)
})
export type ToolSchema = z.infer<typeof toolSchema>