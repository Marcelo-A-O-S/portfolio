import z from "zod";
import { categoryContentSchema } from "./CategoryContentSchema";
export const categorySchema = z.object({
    id: z.string().optional(),
    categoriesContents: z.array(categoryContentSchema)
})
export type CategorySchema = z.infer<typeof categorySchema>;