import z from "zod";
import { categoryContentSchema } from "./CategoryContentSchema";
export const categorySchema = z.object({
    id: z.uuid().optional(),
    categoryContents: z.array(categoryContentSchema)
})
export type CategorySchema = z.infer<typeof categorySchema>;