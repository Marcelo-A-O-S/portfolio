import z from "zod";
export const toolSchema = z.object({
    id: z.string().nonempty(),
    name: z.string().nonempty(),
    description: z.string().nonempty(),
    content: z.string().nonempty(),
    slug: z.string().nonempty()
})
export type ToolSchema = z.infer<typeof toolSchema>