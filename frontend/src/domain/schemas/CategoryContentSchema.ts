import z from "zod";
export const categoryContentSchema = z.object({
    id: z.string().optional(),
    language: z.string().nonempty(),
    name: z.string().nonempty(),
    slug: z.string().nonempty()
})
export type CategoryContentSchema = z.infer<typeof categoryContentSchema>; 