import z from "zod";

export const contributorFilters = z.object({
    page: z.number(),
    postId: z.uuid(),
    search: z.string().optional()
})
export type contributorFilters = z.infer<typeof contributorFilters>;