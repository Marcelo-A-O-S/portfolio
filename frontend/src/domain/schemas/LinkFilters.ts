import z from "zod";

export const linkFilters = z.object({
    page: z.number(),
    tooId: z.uuid().optional(),
    postId: z.uuid().optional(),
    search: z.string().optional(),
})
export type LinkFilters = z.infer<typeof linkFilters>;