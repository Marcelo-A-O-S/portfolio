import z from "zod";

export const postsFilters = z.object({
    page: z.number(),
    search: z.string().optional()
})
export type PostsFilters = z.infer<typeof postsFilters> 