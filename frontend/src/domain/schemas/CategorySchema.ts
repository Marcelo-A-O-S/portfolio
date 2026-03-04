import z from "zod";
export const categorySchema = z.object({
    id: z.string().optional(),
    name: z.string().nonempty()
})
export type CategorySchema = z.infer<typeof categorySchema>;