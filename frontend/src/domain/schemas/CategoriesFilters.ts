import z from "zod";

export const categoriesFilters = z.object({
    page: z.number().nonoptional(),
    language: z.string().optional(),
    search: z.string().optional(),
})
export type CategoriesFilters = z.infer<typeof categoriesFilters>;