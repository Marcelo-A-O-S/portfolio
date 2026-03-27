import z from "zod";

export const postContentSchema = z.object({
    id: z.uuid().optional(),
    languageId: z.uuid().optional(),
    title: z.string().nonempty(),
    description: z.string().nonempty(),
    content: z.string().nonempty(),
    slug: z.string().nonempty(),
})
export type PostContentSchema = z.infer<typeof postContentSchema>;