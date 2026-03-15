import z from "zod";

export const toolContentSchema = z.object({
    id: z.uuid().optional(),
    toolId: z.uuid().optional(),
    name: z.string().nonempty(),
    description: z.string().nonempty(),
    content: z.string().nonempty(),
    slug: z.string().nonempty(),
    language: z.string().nonempty()
})
export type ToolContentSchema = z.infer<typeof toolContentSchema>